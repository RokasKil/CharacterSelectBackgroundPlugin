using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Text;
using Lumina.Excel.Sheets;
using TitleEdit.Data.Character;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Extensions;
using TitleEdit.Utility;
using Character = FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace TitleEdit.PluginServices.Lobby
{
    public unsafe partial class LobbyService
    {
        private delegate void UpdateCharaSelectDisplayDelegate(IntPtr agentLobby, int p2, byte p3);

        private delegate nint CreateBattleCharacterDelegate(nint objectManager, uint index, byte assignCompanion);

        private delegate void SetCharSelectCurrentWorldDelegate(ulong p1, byte p2);

        private delegate void CharSelectWorldPreviewEventHandlerDelegate(ulong p1, ulong p2, ulong p3, uint p4);

        private Hook<UpdateCharaSelectDisplayDelegate> updateCharaSelectDisplayHook = null!;
        private Hook<CreateBattleCharacterDelegate> createBattleCharacterHook = null!;
        private Hook<SetCharSelectCurrentWorldDelegate> setCharSelectCurrentWorldHook = null!;
        private Hook<CharSelectWorldPreviewEventHandlerDelegate> charSelectWorldPreviewEventHandlerHook = null!;

        public Character* CurrentCharacter => FFXIVClientStructs.FFXIV.Client.UI.Agent.CharaSelectCharacterList.GetCurrentCharacter();
        public CharaSelectCharacterList* CharaSelectCharacterList => FFXIVClientStructs.FFXIV.Client.UI.Agent.CharaSelectCharacterList.Instance();
        public ulong CurrentContentId => AgentLobby->HoveredCharacterContentId;

        // A flag if we should modify CreateBattleCharacter behaviour to forcefully include mounts and set position
        private bool creatingCharSelectGameObjects = false;

        // Used to check if character switched in UpdateCharacter method
        private ulong lastContentId;

        // Used to restore relative character rotation after switching scenes, rendering new characters
        private float lastCharacterRotation = 0;

        private void HookCharacter()
        {
            // Called every frame and is responsible for switching out currently selected character
            // We had proper methods to hook but they got inlined with release of DT so now we're polling
            // Client::UI::Agent::AgentLobby.UpdateCharaSelectDisplay
            updateCharaSelectDisplayHook = Hook<UpdateCharaSelectDisplayDelegate>("E8 ?? ?? ?? ?? 84 C0 74 ?? C6 87 ?? ?? ?? ?? ?? 80 BF ?? ?? ?? ?? ?? 74", UpdateCharaSelectDisplayDetour);

            // Called when the game is making a new character - if set by other hooks we force the flag to include a companionObject so we can display a mount
            createBattleCharacterHook = Hook<CreateBattleCharacterDelegate>("E8 ?? ?? ?? ?? 8B D0 41 89 44", CreateBattleCharacterDetour);

            // Called when you load into character select, select a new world in character select or cancel selection so it reload the current
            // we use it make sure characters get created with a companion slots,
            setCharSelectCurrentWorldHook = Hook<SetCharSelectCurrentWorldDelegate>("E8 ?? ?? ?? ?? 8B 84 24 ?? ?? ?? ?? 83 F8 ?? 75", SetCharSelectCurrentWorldDetour);

            // Happens on world list hover when loading a world - we use it make sure characters get created with a companion slots (
            charSelectWorldPreviewEventHandlerHook = Hook<CharSelectWorldPreviewEventHandlerDelegate>("E8 ?? ?? ?? ?? 49 8B CF E8 ?? ?? ?? ?? 41 0F B6 87", CharSelectWorldPreviewEventHandlerDetour);
        }

        // Called every frame and is responsible for switching out currently selected character
        private void UpdateCharaSelectDisplayDetour(nint agentLobby, int p2, byte p3)
        {
            //Services.Log.Debug($"UpdateCharaSelectDisplayDetour {p2}, {p3}");
            var preUpdateCharacter = CurrentCharacter;
            updateCharaSelectDisplayHook.Original(agentLobby, p2, p3);
            if (preUpdateCharacter != CurrentCharacter)
            {
                Services.Log.Debug($"CurrentChar changed {(IntPtr)preUpdateCharacter:X} - {(IntPtr)CurrentCharacter:X}");
                if (lastContentId == 0)
                {
                    Services.Log.Debug($"Resetting last character rotation {lastCharacterRotation}");
                }

                UpdateCharacter();
            }
        }

        // We do polling cause it's simpler than figuring when exactly are mounts and stuff are good to draw
        private void CharacterTick()
        {
            if (CurrentLobbyMap == GameLobbyType.CharaSelect)
            {
                if (CurrentCharacter != null)
                {
                    // Don't record rotation when loading next scene
                    if (!rotationJustRecorded && !resetCharacterSelectScene)
                    {
                        lastCharacterRotation = Utils.NormalizeAngle(CurrentCharacter->Rotation - characterSelectLocationModel.Rotation);
                    }

                    // if needed we force draw character and mount cause they're weird sometimes
                    if (CurrentCharacter->GameObject.RenderFlags != 0 && CurrentCharacter->GameObject.RenderFlags != (VisibilityFlags)0x40 && CurrentCharacter->GameObject.IsReadyToDraw())
                    {
                        Services.Log.Debug($"Drawing character {(nint)CurrentCharacter:X} {CurrentCharacter->GameObject.RenderFlags:X}");
                        CurrentCharacter->GameObject.EnableDraw();
                        if (CurrentCharacter->IsMounted() && CurrentCharacter->CompanionObject != null && CurrentCharacter->CompanionObject->Character.GameObject.IsReadyToDraw())
                        {
                            Services.Log.Debug($"Drawing companion {(nint)CurrentCharacter->CompanionObject:X} {CurrentCharacter->CompanionObject->Character.GameObject.RenderFlags:X}");
                            CurrentCharacter->CompanionObject->Character.GameObject.EnableDraw();
                        }
                    }
                }
            }
        }

        // Get currently loaded character select characters to store for use in configuration window
        public Dictionary<ulong, string> GetCurrentCharacterNames()
        {
            Dictionary<ulong, string> result = [];
            if (CurrentLobbyMap != GameLobbyType.CharaSelect) return result;
            var characterSelects = AgentLobby->LobbyData.CharaSelectEntries.AsSpan();
            foreach (var character in characterSelects)
            {
                if (character.Value->ContentId != 0)
                {
                    if (Services.DataManager.GetExcelSheet<World>().TryGetRow(character.Value->HomeWorldId, out var world))
                    {
                        result[character.Value->ContentId] = $"{Encoding.UTF8.GetString(character.Value->Name).TrimEnd('\0')}@{world.Name}";
                    }
                }
            }

            return result;
        }

        // Called when you select a new world in character select or cancel selection so it reload the current
        // if p2 is 1 it doesn't recreate the characters or something along that line it was added in 7.16
        private void SetCharSelectCurrentWorldDetour(ulong p1, byte p2)
        {
            Services.Log.Debug("SetCharSelectCurrentWorldDetour");
            creatingCharSelectGameObjects = true;
            setCharSelectCurrentWorldHook.Original(p1, p2);
            creatingCharSelectGameObjects = false;
            foreach (var entry in GetCurrentCharacterNames())
            {
                Services.CharactersService.PutCharacter(entry.Key, entry.Value);
            }

            Services.CharactersService.SaveCharacters();

            *FFXIVClientStructs.FFXIV.Client.UI.Agent.CharaSelectCharacterList.StaticAddressPointers.ppGetCurrentCharacter = GetCurrentHoveredCharacter();
            Services.Log.Debug($"Set current char to {(nint)(*FFXIVClientStructs.FFXIV.Client.UI.Agent.CharaSelectCharacterList.StaticAddressPointers.ppGetCurrentCharacter):X}");
            UpdateCharacter(true);
            RotateCharacter();
        }

        // When loading a new scene force all character positions to the new location to avoid some camera jank when switching selected characters
        private void SetAllCharacterPostions()
        {
            for (int i = 0; i < CharaSelectCharacterList->CharacterMapping.Length; i++)
            {
                if (CharaSelectCharacterList->CharacterMapping[i].ContentId == 0)
                {
                    break;
                }

                var gameObject = ClientObjectManager->GetObjectByIndex((ushort)CharaSelectCharacterList->CharacterMapping[i].ClientObjectIndex);
                if (gameObject != null)
                {
                    Services.Log.Debug($"Setting position for {(IntPtr)gameObject:X}");
                    gameObject->SetPosition(characterSelectLocationModel.Position);
                }
            }
        }

        // Restores character rotation on scene loads or newly created character objects
        private void RotateCharacter()
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter->GameObject.SetRotation(Utils.NormalizeAngle(lastCharacterRotation + characterSelectLocationModel.Rotation));
            }
        }

        // Used to reset state when unloading
        private void ResetCharacters()
        {
            for (int i = 0; i < CharaSelectCharacterList->CharacterMapping.Length; i++)
            {
                if (CharaSelectCharacterList->CharacterMapping[i].ContentId == 0)
                {
                    break;
                }

                var clientObjectIndex = CharaSelectCharacterList->CharacterMapping[i].ClientObjectIndex;
                var gameObject = ClientObjectManager->GetObjectByIndex((ushort)clientObjectIndex);
                if (gameObject != null)
                {
                    gameObject->SetPosition(0, 0, 0);
                    ((Character*)gameObject)->Mount.CreateAndSetupMount(0, 0, 0, 0, 0, 0, 0);
                }
            }
        }

        // Load the Nothing selected location
        private void NothingSelected()
        {
            Services.Log.Debug("Nothing selected");
            lastContentId = 0;
            var newLocationModel = GetNothingSelectedLocation();
            if (!newLocationModel.Equals(characterSelectLocationModel))
            {
                previousCharacterSelectModelRotation = characterSelectLocationModel.Rotation;
                characterSelectLocationModel = newLocationModel;
                resetCharacterSelectScene = true;
            }
        }

        // When hovering over world this method creates the newly shown character 
        // temporarily set creatingCharSelectGameObjects to true so CreateBattleCharacterDetour activates
        private void CharSelectWorldPreviewEventHandlerDetour(ulong p1, ulong p2, ulong p3, uint p4)
        {
            Services.Log.Debug($"[CharSelectWorldPreviewEventHandlerDetour] {p1:X} {p2:X} {p3:X} {p4:X}");
            creatingCharSelectGameObjects = true;
            charSelectWorldPreviewEventHandlerHook.Original(p1, p2, p3, p4);
            creatingCharSelectGameObjects = false;
        }

        // Make sure characters are created with companion slot (for mounts) and positioned properly
        private nint CreateBattleCharacterDetour(nint objectManager, uint index, byte assignCompanion)
        {
            var result = createBattleCharacterHook.Original(objectManager, index, creatingCharSelectGameObjects ? (byte)1 : assignCompanion);
            if (creatingCharSelectGameObjects)
            {
                // When making a new character make sure it's created with a companion and also prematurely set it's position to avoid some camera jank
                Services.Log.Debug($"[CreateBattleCharacterDetour] setting assignCompanion {index:X} {result:X}");
                ClientObjectManager->GetObjectByIndex((ushort)result)->SetPosition(characterSelectLocationModel.Position.X, characterSelectLocationModel.Position.Y, characterSelectLocationModel.Position.Z);
            }

            return result;
        }

        //Get current hovered character by it's content id because the index is set to 100 when flipping through worlds
        private Character* GetCurrentHoveredCharacter()
        {
            if (CurrentContentId == 0)
            {
                return null;
            }

            for (var i = 0; i < CharaSelectCharacterList->CharacterMapping.Length; i++)
            {
                if (CharaSelectCharacterList->CharacterMapping[i].ContentId == CurrentContentId)
                {
                    return (Character*)ClientObjectManager->GetObjectByIndex((ushort)CharaSelectCharacterList->CharacterMapping[i].ClientObjectIndex);
                }
            }

            return null;
        }

        // New character is being shown or the current one needs an update
        // Sets position, rotation, mount checks if a new location needs to be picked and reloads it
        private void UpdateCharacter(bool forced = false)
        {
            if (CurrentCharacter != null)
            {
                if (lastContentId != CurrentContentId || forced)
                {
                    lastContentId = CurrentContentId;

                    var newLocationModel = GetLocationForContentId(CurrentContentId);
                    if (!newLocationModel.Equals(characterSelectLocationModel))
                    {
                        previousCharacterSelectModelRotation = characterSelectLocationModel.Rotation;
                        characterSelectLocationModel = newLocationModel;
                        resetCharacterSelectScene = true;
                    }
                    else
                    {
                        RotateCharacter();
                    }

                    Services.Log.Debug($"Setting character position {(nint)CurrentCharacter:X}");
                    CurrentCharacter->GameObject.SetPosition(characterSelectLocationModel.Position.X, characterSelectLocationModel.Position.Y, characterSelectLocationModel.Position.Z);

                    ((CharacterExpanded*)CurrentCharacter)->MovementMode = characterSelectLocationModel.MovementMode;
                    if (CurrentCharacter->Mount.MountId != characterSelectLocationModel.Mount.MountId)
                    {
                        SetupMount(CurrentCharacter, CurrentContentId, characterSelectLocationModel);
                    }
                }
            }
            else
            {
                NothingSelected();
            }
        }

        // Setup mount for character by calling native method
        private void SetupMount(Character* character, ulong contentId, LocationModel location)
        {
            var mount = location.Mount.LastLocationMount ? Services.LocationService.GetLocationModel(contentId).Mount : location.Mount;
            character->Mount.CreateAndSetupMount(
                (short)mount.MountId,
                mount.BuddyModelTop,
                mount.BuddyModelBody,
                mount.BuddyModelLegs,
                mount.BuddyStain,
                0, 0);
        }
    }
}
