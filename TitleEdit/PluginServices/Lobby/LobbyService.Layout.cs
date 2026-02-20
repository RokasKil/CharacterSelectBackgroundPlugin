using System;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using System.Collections.Generic;
using TitleEdit.Data.Layout;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Extensions;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Lobby
{
    public unsafe partial class LobbyService
    {
        [Signature("40 53 48 83 EC ?? 44 0F B7 C1")]
        private readonly delegate* unmanaged<ushort, void> setTimeNative = null!;

        private delegate void CharSelectSetWeatherDelegate();

        private Hook<CharSelectSetWeatherDelegate> lobbySetWeatherHook = null!;

        private delegate void LayoutManagerInitWeatherDelegate(IntPtr layoutManagerEnvironment);

        private delegate void LayoutManagerInitHousingDelegate(LayoutManager* layoutManager, ushort p2);

        private Hook<LayoutManagerInitWeatherDelegate> layoutManagerInitWeatherHook = null!;

        private Hook<LayoutManagerInitHousingDelegate> layoutManagerInitHousingHook = null!;

        private bool forceUpdateCharacter;

        private void HookLayout()
        {
            // Called when game does some lobby weather setting - we use it as an indicator to set scene details like weather, time and layout
            // Called on scene load and on displayed character switch
            lobbySetWeatherHook = Hook<CharSelectSetWeatherDelegate>("E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? B3 ?? E9", LobbySetWeatherDetour);
            // Sets the initial weather on scene load, we change the weather here early to avoid the ugly weather transitions
            // We still have to fight the game resetting that in LobbySetWeatherDetour, also use this to load in furniture to prevent pop-in
            layoutManagerInitWeatherHook = Hook<LayoutManagerInitWeatherDelegate>("E8 ?? ?? ?? ?? 83 BD ?? ?? ?? ?? ?? 74 ?? 48 8B 9D", LayoutManagerInitWeatherDetour);
            // Initializes something with the OutdoorAreaLayoutData structs, we use this to set outside housing plots to minimize pop-in
            layoutManagerInitHousingHook = Hook<LayoutManagerInitHousingDelegate>("40 57 48 83 EC ?? 48 8B B9 ?? ?? ?? ?? 4C 8B C7", LayoutManagerInitHousingDetour);
        }

        private bool TryGetCurrentLocationModel(out LocationModel model, bool alwaysGetTitle = false)
        {
            if (CurrentLobbyMap == GameLobbyType.CharaSelect)
            {
                model = characterSelectLocationModel;
            }
            else if (CurrentLobbyMap == GameLobbyType.Title && (alwaysGetTitle || ShouldModifyTitleScreen))
            {
                model = titleScreenLocationModel;
            }
            else
            {
                model = default;
                return false;
            }

            return true;
        }

        private void LayoutManagerInitWeatherDetour(IntPtr layoutManagerEnvironment)
        {
            Services.Log.Debug($"[LayoutManagerInitWeatherDetour] {layoutManagerEnvironment:X16}");
            layoutManagerInitWeatherHook.Original(layoutManagerEnvironment);

            if (TryGetCurrentLocationModel(out var model))
            {
                Services.WeatherService.WeatherId = model.WeatherId;
                LoadEstate(model);
                LoadFurniture(model);
            }
        }

        private void LayoutManagerInitHousingDetour(LayoutManager* layoutManager, ushort p2)
        {
            Services.Log.Debug($"[LayoutManagerInitHousingDetour] {(IntPtr)layoutManager:X16} {p2}");
            layoutManagerInitHousingHook.Original(layoutManager, p2);
            if (TryGetCurrentLocationModel(out var model))
            {
                LoadPlots(model);
            }
        }

        public void HousingTest()
        {
            if (TryGetCurrentLocationModel(out var model))
            {
                InitializeHousingLayout(model);
                Services.Framework.RunOnTick(() =>
                {
                    LoadEstate(model);
                    LoadFurniture(model);
                    LoadPlots(model);
                }, delayTicks: 1);
            }
        }

        private void LobbySetWeatherDetour()
        {
            lobbySetWeatherHook.Original();
            SetLayoutInfo();
            if (forceUpdateCharacter)
            {
                UpdateCharacter(true);
                forceUpdateCharacter = false;
            }

            Services.Log.Debug($"LobbySetWeatherDetour {Services.WeatherService.WeatherId}");
        }

        private void SetLayoutInfo()
        {
            if (!TryGetCurrentLocationModel(out var model))
            {
                return;
            }

            if (model is { SaveFestivals: true, Festivals: not null })
            {
                fixed (GameMain.Festival* pFestivals = new GameMain.Festival[LocationModel.FESTIVAL_COUNT])
                {
                    for (int i = 0; i < LocationModel.FESTIVAL_COUNT; i++)
                    {
                        pFestivals[i].Id = model.Festivals[i].Id;
                        pFestivals[i].Phase = model.Festivals[i].Phase;
                    }

                    Services.LayoutService.LayoutManager->SetActiveFestivals(pFestivals);
                }
            }
            else
            {
                Services.Log.Debug("Unsetting festivals");
                fixed (GameMain.Festival* pFestivals = new GameMain.Festival[LocationModel.FESTIVAL_COUNT])
                {
                    Services.LayoutService.LayoutManager->SetActiveFestivals(pFestivals);
                }
            }

            Services.WeatherService.WeatherId = model.WeatherId;
            SetTime(model);
            Services.Log.Debug($"SetWeather to {Services.WeatherService.WeatherId}");
            if (model is { SaveLayout: true, Active: not null, Inactive: not null })
            {
                List<ulong> unknownUUIDs = new();
                Services.LayoutService.ForEachInstance(instance =>
                {
                    if (model.Active.Contains(instance.Value->UUID()))
                    {
                        SetActive(instance.Value, 1, model);
                    }
                    else if (model.Inactive.Contains(instance.Value->UUID()))
                    {
                        SetActive(instance.Value, 0, model);
                    }
                    else
                    {
                        unknownUUIDs.Add(instance.Value->UUID());
                    }
                });
                if (unknownUUIDs.Count > 0)
                {
                    Services.Log.Debug($"{unknownUUIDs.Count} UUIDs not found in the layout data");
                }
            }
        }

        private void SetActive(ILayoutInstance* instance, byte active, LocationModel model)
        {
            if (instance->Id.Type == InstanceType.Vfx)
            {
                if (model.UseVfx)
                {
                    SetIndex((VfxLayoutInstance*)instance, model);
                    instance->SetActiveVf54(active);
                }
            }
            else
            {
                instance->SetActive(active != 0);
            }
        }

        // Sets vfx trigger index, defines which track of the vfx to play (maybe)
        private void SetIndex(VfxLayoutInstance* instance, LocationModel model)
        {
            if (model.VfxTriggerIndexes.TryGetValue(instance->ILayoutInstance.UUID(), out var index) && index != instance->VfxTriggerIndex)
            {
                Services.LayoutService.SetVfxLayoutInstanceVfxTriggerIndex(instance, index);
            }
        }

        private void SetTime(LocationModel model)
        {
            SetTime(model.UseLiveTime ? Services.LocationService.TimeOffset : model.TimeOffset);
        }

        private void SetTime(ushort time) => setTimeNative(time);
    }
}
