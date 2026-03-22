using Dalamud.Hooking;
using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.Interop;
using Lumina.Models.Models;
using TitleEdit.Data.BGM;
using TitleEdit.Data.Character;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Lobby
{
    public unsafe partial class LobbyService
    {
        [Signature("E8 ?? ?? ?? ?? 0F 57 C0 45 0F 57 E4")]
        private readonly delegate* unmanaged<CharacterSetupContainer*, uint, void> setupEventNpc = null!;
        private ClientObjectManager* ClientObjectManager => FFXIVClientStructs.FFXIV.Client.Game.Object.ClientObjectManager.Instance();
        private List<ushort> spawnedNpcs = [];
        private Dictionary<ushort, NpcModel> spawningNpcs = [];


        private void HookNpcs()
        {
            Services.LayoutService.OnLayoutChange += OnLayoutChange;
        }

        // Setup takes a few frames to initialize (sometimes (on initial load?)) so we have to wait to finalise some properties or else they'll get reset
        private void TickNpcs()
        {
            if (spawningNpcs.Count == 0) return;
            List<ushort> toRemove = [];
            foreach (var keyValuePair in spawningNpcs)
            {
                var index = keyValuePair.Key;
                var npc = keyValuePair.Value;
                var chara = (BattleChara*)ClientObjectManager->GetObjectByIndex(index);
                if (chara == null)
                {
                    Services.Log.Warning($"[TickNpcs] Couldn't find npc {index}");
                    toRemove.Add(index);
                    continue;
                }

                if (chara->DrawObject != null)
                {
                    chara->SetPosition(npc.Position.X, npc.Position.Y, npc.Position.Z);
                    chara->SetRotation(npc.Rotation);

                    ((CharacterExpanded*)chara)->SetScale(npc.Scale);
                    chara->Character.GameObject.EnableDraw();
                    Services.Log.Debug($"[TickNpcs] Finalized npc {index}");
                    toRemove.Add(index);
                }
            }

            foreach (var index in toRemove)
            {
                spawningNpcs.Remove(index);
            }
        }

        private void SpawnNpcs(LocationModel model)
        {
            // Do checks 
            if (!Services.ConfigurationService.SeasonalEasterEggs ||
                model.Npcs is not { Count: > 0 } ||
                (DateTime.Now is not { Month: 4, Day: >= 1, Day: <= 2 } && !Services.ConfigurationService.IgnoreSeasonalDateCheck)) return;

            foreach (var npc in model.Npcs)
            {
                var index = (ushort)ClientObjectManager->CreateBattleCharacter();
                if (index == 0xFFFF)
                    return;
                Services.Log.Debug($"[SpawnNpcs] Spawning eNpc: {index}, position: {npc.Position}, rotation: {npc.Rotation}, npcBase: {npc.ENpcId}");
                var chara = (BattleChara*)ClientObjectManager->GetObjectByIndex(index);
                spawnedNpcs.Add(index);
                Services.Log.Debug($"[SpawnNpcs] eNpc {(IntPtr)chara:X}");
                setupEventNpc(&chara->CharacterSetup, npc.ENpcId);
                spawningNpcs[index] = npc;
                chara->Character.GameObject.EnableDraw();
            }
        }

        private void DespawnNpcs()
        {
            if (spawnedNpcs.Count == 0) return;
            foreach (var index in spawnedNpcs)
            {
                if (ClientObjectManager->GetObjectByIndex(index) != null)
                {
                    Services.Log.Debug($"[DespawnNpcs] Despawning npc {index}");
                    ClientObjectManager->DeleteObjectByIndex(index, 0);
                }
                else
                {
                    Services.Log.Warning($"[DespawnNpcs] Couldn't find npc {index}");
                }
            }

            spawnedNpcs = [];
            spawningNpcs = [];
        }

        private void OnLayoutChange()
        {
            DespawnNpcs();
            // The LobbyUiStage check is here for the Dawntrail screen because it doesn't use the same logic as every other title screen
            if (LobbyUiStage == LobbyUiStage.LoadingTitleScreen1 || LobbyType == GameLobbyType.Title)
            {
                // Spawn npcs even if it's a vanilla title screen
                if (titleScreenLoaded)
                {
                    Services.Log.Debug($"[OnLayoutChange] Spawning title npcs");
                    SpawnNpcs(titleScreenLocationModel);
                }
            }
            else if (LobbyType == GameLobbyType.CharaSelect)
            {
                Services.Log.Debug($"[OnLayoutChange] Spawning chara npcs");
                SpawnNpcs(characterSelectLocationModel);
            }
        }

        private void DisposeNpcs()
        {
            Services.LayoutService.OnLayoutChange -= OnLayoutChange;
            DespawnNpcs();
        }
    }
}