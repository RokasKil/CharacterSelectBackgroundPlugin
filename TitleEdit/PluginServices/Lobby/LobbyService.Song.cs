using Dalamud.Hooking;
using System;
using TitleEdit.Data.BGM;
using TitleEdit.Data.Lobby;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Lobby
{
    public unsafe partial class LobbyService
    {
        private delegate nint PlayMusicDelegate(LobbyInfo* self, string filename, float volume, uint fadeTime);

        private delegate void PickSongDelegate(LobbyInfo* self, LobbySong* musicIndex);

        private delegate void StopMusicDelegate(LobbyInfo* self);

        private Hook<PlayMusicDelegate> playMusicHook = null!;
        private Hook<PickSongDelegate> pickSongHook = null!;
        private Hook<StopMusicDelegate> stopMusicHook = null!;

        // Used to know if we should reset music between reloads or not
        private string? lastBgmPath;

        private LobbySong lastMusicIndex;
        private bool changeBgm;

        private void HookSong()
        {
            // Called when a different lobby (and some other) music needs to be loaded - we force call the game to call it by resetting the CurrentLobbyMusicIndex value
            playMusicHook = Hook<PlayMusicDelegate>("E8 ?? ?? ?? ?? 48 89 47 18 89 5F 20", PlayMusicDetour);
            // Called when game switches lobby music to a different type -
            pickSongHook = Hook<PickSongDelegate>("E8 ?? ?? ?? ?? 33 C9 E8 ?? ?? ?? ?? 48 8B 0D", PickSongDetour);
            // Called when game wants to turn off lobby music when moving back to title screen at LobbyUiStage.UnloadingCharacterSelect2 
            // I don't really see a point to this call because the title screen will call PickSong to play it's own music
            // I NOP it so I can keep the music going if the BGM was the same between char select and title screen
            stopMusicHook = Hook<StopMusicDelegate>("E8 ?? ?? ?? ?? EB ?? 8B CB E8 ?? ?? ?? ?? C6 86", StopMusicDetour);
        }

        private void StopMusicDetour(LobbyInfo* self)
        {
            Services.Log.Debug($"[StopMusicDetour] {(nint)self:X} {CurrentLobbyMap} {LobbyUiStage} {LobbyInfo->CurrentLobbyMusicIndex} ");
        }

        private void PickSongDetour(LobbyInfo* self, LobbySong* musicIndex)
        {
            Services.Log.Debug($"[PickSongDetour] {(nint)self:X} {*musicIndex}");
            lastMusicIndex = *musicIndex;
            // play new music if DontInterruptMusicOnSceneSwitch is turned off or there is no music playing currently or we want to play a different track or live editing is enabled
            if (!Services.ConfigurationService.DontInterruptMusicOnSceneSwitch ||
                LobbyInfo->BgmPointer == 0 ||
                GetBgmPath(*musicIndex) != lastBgmPath ||
                (*musicIndex == LobbySong.CharacterSelect && liveEditCharacterSelect) ||
                (IsTitleScreenMusic(*musicIndex) && liveEditTitleScreen))
            {
                Services.Log.Debug($"[PickSongDetour] loading different music");
                changeBgm = true;
                pickSongHook.Original(self, musicIndex);
                changeBgm = false;
            }
            else
            {
                Services.Log.Debug($"[PickSongDetour] same bgm path {lastBgmPath}");
                LobbyInfo->CurrentLobbyMusicIndex = *musicIndex;
            }
        }

        private static bool IsTitleScreenMusic(LobbySong musicIndex) => musicIndex is LobbySong.ARealmRebornTitle
                                                                            or LobbySong.HeavensWardTitle
                                                                            or LobbySong.StormbloodTitle
                                                                            or LobbySong.ShadowbringersTitle
                                                                            or LobbySong.EndwalkerTitle
                                                                            or LobbySong.DawntrailTitle;

        private String? GetBgmPath(LobbySong musicIndex)
        {
            if (shouldReloadTitleScreenOnLoadingStage2)
            {
                return "music/ffxiv/BGM_Null.scd";
            }

            if (musicIndex == LobbySong.CharacterSelect)
            {
                if (CurrentLobbyMap == GameLobbyType.CharaSelect)
                {
                    return characterSelectLocationModel.BgmPath;
                }
            }
            else if (IsTitleScreenMusic(musicIndex) && ShouldModifyTitleScreen)
            {
                return titleScreenLocationModel.BgmPath;
            }

            return musicIndex switch
            {
                LobbySong.None => null,
                LobbySong.CharacterSelect => "music/ffxiv/BGM_System_Chara.scd",
                LobbySong.ARealmRebornTitle => "music/ffxiv/BGM_System_Title.scd",
                LobbySong.HeavensWardTitle => "music/ex1/BGM_EX1_System_Title.scd",
                LobbySong.StormbloodTitle => "music/ex2/BGM_EX2_System_Title.scd",
                LobbySong.ShadowbringersTitle => "music/ex3/BGM_EX3_System_Title.scd",
                LobbySong.EndwalkerTitle => "music/ex4/BGM_EX4_System_Title.scd",
                LobbySong.DawntrailTitle => "music/ex5/BGM_EX5_System_Title.scd",
                _ => ""
            };
        }

        // Sets songIndex to None and allows the game itself to restart music when it's appropiate
        private void ResetSongIndex()
        {
            Services.Log.Debug("ResetSongIndex");
            LobbyInfo->CurrentLobbyMusicIndex = LobbySong.None;
        }

        // Sets songIndex to None and calls the PickSong method to start playing
        private void ForcePlaySongIndex(LobbySong song)
        {
            LobbyInfo->CurrentLobbyMusicIndex = LobbySong.None;
            PickSong(song);
        }

        // Picks the lobby bgm depending on the provided songIndex, won't do anything if it thinks this songIndex is already playing
        private void PickSong(LobbySong songIndex)
        {
            lastMusicIndex = songIndex;
            changeBgm = true;
            pickSongHook.Original((LobbyInfo*)lobbyStructAddress, &songIndex);
            changeBgm = false;
        }

        // TODO: figure out looping on tracks that don't loop
        private nint PlayMusicDetour(LobbyInfo* self, string filename, float volume, uint fadeTime)
        {
            Services.Log.Debug($"PlayMusicDetour {LobbyUiStage} {LobbyInfo->CurrentLobbyMusicIndex} {(nint)self:X} {filename} {volume} {fadeTime}");
            if (changeBgm)
            {
                lastBgmPath = filename = GetBgmPath(lastMusicIndex) ?? "music/ffxiv/BGM_Null.scd";
                Services.Log.Debug($"Setting music to {filename}");
            }

            return playMusicHook.Original(self, filename, volume, fadeTime);
        }
    }
}
