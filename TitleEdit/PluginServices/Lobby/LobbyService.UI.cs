using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Hooking;
using Dalamud.Interface.ImGuiNotification;
using Dalamud.Utility.Numerics;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Lobby
{
    public unsafe partial class LobbyService
    {
        [Signature("E8 ?? ?? ?? ?? 48 8B CF E8 ?? ?? ?? ?? 48 8B CF E8 ?? ?? ?? ?? E8")]
        private readonly delegate* unmanaged<AgentLobby*, void> removeTitleScreenUi = null!;
        private readonly string[] recolorableAddons = { "_TitleRights", "_TitleMenu", "_TitleRevision" };

        private delegate bool PickTitleLogo(AtkUnitBase* atkUnit);

        private delegate int TitleLogoRefresh(AtkUnitBase* atkUnit, ulong p2, AtkValue* atkValue);

        private delegate int ShowAddon(AtkUnitBase* atkUnit, byte p2, int p3);

        private Hook<PickTitleLogo> pickTitleLogoHook = null!;
        private Hook<TitleLogoRefresh> titleLogoRefreshHook = null!;
        private Hook<ShowAddon> showAddonHook = null!;


        // Should play dawntrail logo animation once everything loads 
        private bool shouldAnimateDawntrailLogo = false;

        // Should advance title logo animation once everything loads, useful for Endwalker which shows up only after 10+ seconds
        private bool shouldAdvanceTitleLogoAnimation = false;
        private float advanceTitleLogoAnimationDuration = 0;

        // Should hide the whole TitleLogo addon, we reenable it when dawntrail cutscene or going to character select triggers an animation
        private bool shouldHideTitleLogoAddon = false;

        // Are we reloading title screen ui
        private bool reloadingTitleScreenUi = false;

        // Did DT cutscene trigger logo animation used 
        private bool cutsceneAnimatedTitleLogo = false;

        private Queue<Action> titleMenuFinalizeActions = [];


        // Called to check if we need to modify title screen logo we always handle things because free trial client will always want to show free trial logo
        private bool ShouldModifyTitleScreenLogo => titleScreenLoaded;

        // Called to check if we need to modify title screen ui colors we handle things when not using a vanilla screen or the colors need to overriden globally
        private bool ShouldModifyTitleScreenUiColors => titleScreenLoaded && (titleScreenLocationModel.TitleScreenOverride == null || Services.ConfigurationService.OverridePresetTitleScreenColor);

        //These might be inneficient but we don't call them often so maybe it's fine or maybe it's smart and caches but I doubt
        private UiColorModel TitleScreenColorsOption
        {
            get
            {
                UiColorModel color;
                if (titleScreenLocationModel.UiColor.Expansion == UiColorExpansion.Unspecified ||
                    (Services.ConfigurationService.OverridePresetTitleScreenColor && !liveEditTitleScreen))
                {
                    color = Services.ConfigurationService.TitleScreenColor;
                }
                else
                {
                    color = titleScreenLocationModel.UiColor;
                }

                return color.Expansion == UiColorExpansion.Custom ? color : UiColors.GetColorModelByExpansion(color.Expansion);
            }
        }

        private TitleScreenLogo TitleScreenLogoOption
        {
            get
            {
                if (titleScreenLocationModel.TitleScreenLogo == TitleScreenLogo.Unspecified ||
                    (Services.ConfigurationService.OverridePresetTitleScreenLogo && !liveEditTitleScreen))
                {
                    return Services.ConfigurationService.TitleScreenLogo;
                }
                else
                {
                    return titleScreenLocationModel.TitleScreenLogo;
                }
            }
        }

        private void HookUi()
        {
            // Called to pick what logo to display for the _TitleLogo addon (one of the vfuncs)
            pickTitleLogoHook = Hook<PickTitleLogo>("40 57 48 83 EC ?? 48 8B F9 E8 ?? ?? ?? ?? 80 78 ?? ?? 74", PickTitleLogoDetour);

            // Called to play animations for the _TitleLogo addon (one of the vfuncs)
            titleLogoRefreshHook = Hook<TitleLogoRefresh>("48 89 5C 24 ?? 56 48 83 EC ?? F6 81 ?? ?? ?? ?? ?? 49 8B F0 48 8B D9 0F 84 ?? ?? ?? ?? 49 8B C8 48 89 7C 24 ?? E8 ?? ?? ?? ?? 8B F8 A8 ?? 74 ?? 48 8B 8B ?? ?? ?? ?? BA ?? ?? ?? ?? C7 83", TitleLogoRefreshDetour);

            // Common method that is called when setting an addon to be visible, used to hide _TitleLogo when using dawntrail title screen
            // so it can be shown when TitleLogoRefresh gets called to trigger the logo animation
            // if we don't do this non DT logos will show at the start and then flash when animation triggers
            showAddonHook = Hook<ShowAddon>("E8 ?? ?? ?? ?? 80 A3 ?? ?? ?? ?? ?? 40 80 E7 ?? 40 08 BB ?? ?? ?? ?? EB ?? 44 8B C6", ShowAddonDetour);

            // To animate/skip animation of title logo
            Services.AddonLifecycle.RegisterListener(AddonEvent.PostSetup, "_TitleLogo", TitleLogoPostSetup);

            // To hide character select names
            HideCharacterSelectNamesSettingUpdated();

            // Register UI stuff we want to recolor
            foreach (var addon in recolorableAddons)
            {
                Services.AddonLifecycle.RegisterListener(AddonEvent.PostSetup, addon, RecolorablePostSetup);
            }
        }

        private void TickUi()
        {
            // Used when unloading TitleScreen menu
            // I'm not using RegisterListener with AddonEvent.PreFinalize because there's an edge case where if addon is destroyed early on it doesn't get called
            // not sure what the actual condition but it causes a rare problem where the game boots up without any menus because I called the removeTitleScreenUi
            // but never got a PreFinalize event callback so never restored it
            if (titleMenuFinalizeActions.Count != 0 && Services.GameGui.GetAddonByName("_TitleMenu") == IntPtr.Zero)
            {
                TitleMenuFinalize();
            }
        }

        private int ShowAddonDetour(AtkUnitBase* atkUnit, byte p2, int p3)
        {
            var result = showAddonHook.Original(atkUnit, p2, p3);
            if (shouldHideTitleLogoAddon && atkUnit->NameString == "_TitleLogo")
            {
                Services.Log.Debug("[ShowAddonDetour] Hiding _TitleLogo");
                atkUnit->IsVisible = false;
                shouldHideTitleLogoAddon = false;
            }

            return result;
        }

        private int TitleLogoRefreshDetour(AtkUnitBase* atkUnit, ulong p2, AtkValue* atkValue)
        {
            Services.Log.Debug($"[TitleLogoRefreshDetour] {(IntPtr)atkUnit:X} {p2:X} {(IntPtr)atkValue:X} {atkValue->UInt} {atkValue->Bool}");
            var result = titleLogoRefreshHook.Original(atkUnit, p2, atkValue);
            if (atkValue->UInt == 32 || atkValue->UInt == 1)
            {
                if (atkValue->UInt == 32)
                {
                    cutsceneAnimatedTitleLogo = true;
                }

                atkUnit->IsVisible = true;
            }

            if (shouldAdvanceTitleLogoAnimation && atkValue->UInt == 32)
            {
                AdvanceTitleLogoAnimation();
                shouldAdvanceTitleLogoAnimation = false;
            }

            return result;
        }

        // Hiding names on update, only subscribed if the setting is on
        private void CharaSelectInfoUpdate(AddonEvent type, AddonArgs args) => SetCharaSelectInfoVisibilty(false);
        private void CharaSelectListMenuUpdate(AddonEvent type, AddonArgs args) => SetCharaSelectListMenuVisibilty(false);

        // Set _CharaSelectInfo text node visiblity
        private void SetCharaSelectInfoVisibilty(bool visible)
        {
            var addon = (AtkUnitBase*)Services.GameGui.GetAddonByName("_CharaSelectInfo").Address;
            if (addon == null) return;
            var node = addon->UldManager.SearchNodeById(3);
            if (node == null) return;
            node->ToggleVisibility(visible);
        }


        // Set _CharaSelectListMenu list text node visiblity
        private void SetCharaSelectListMenuVisibilty(bool visible)
        {
            var addon = (AtkUnitBase*)Services.GameGui.GetAddonByName("_CharaSelectListMenu").Address;
            if (addon == null) return;
            var node = addon->UldManager.SearchNodeById(12);
            if (node == null) return;
            var listComponent = node->GetAsAtkComponentList();
            if (listComponent == null) return;
            for (var listItem = listComponent->ItemRendererList; listItem != listComponent->ItemRendererList + listComponent->ListLength; listItem++)
            {
                var listItemTextNode = listItem->AtkComponentListItemRenderer->UldManager.SearchNodeById(6);
                if (listItemTextNode != null)
                {
                    listItemTextNode->ToggleVisibility(visible);
                }
            }
        }

        public void HideCharacterSelectNamesSettingUpdated()
        {
            if (Services.ConfigurationService.HideCharacterSelectNames)
            {
                Services.AddonLifecycle.RegisterListener(AddonEvent.PostUpdate, "_CharaSelectInfo", CharaSelectInfoUpdate);
                Services.AddonLifecycle.RegisterListener(AddonEvent.PostUpdate, "_CharaSelectListMenu", CharaSelectListMenuUpdate);
            }
            else
            {
                Services.AddonLifecycle.UnregisterListener(CharaSelectInfoUpdate);
                Services.AddonLifecycle.UnregisterListener(CharaSelectListMenuUpdate);
                SetCharaSelectInfoVisibilty(true);
                SetCharaSelectListMenuVisibilty(true);
            }
        }

        // TitleScreen Logo has finished setup we animate/modify it if necessary
        private void TitleLogoPostSetup(AddonEvent type, AddonArgs args)
        {
            if (shouldAnimateDawntrailLogo)
            {
                AnimateDawntrailLogo();
                shouldAnimateDawntrailLogo = false;
            }

            // Don't advance animations when shouldHideTitleLogoAddon is true
            // We'll do that when Addon is visible again
            if (shouldAdvanceTitleLogoAnimation && !shouldHideTitleLogoAddon)
            {
                AdvanceTitleLogoAnimation();
                shouldAdvanceTitleLogoAnimation = false;
            }

            // Title screen logos have two parts one with japanese subtitle and one without
            // They all default to part 1 which is the english version even on client set to JP language
            // but shb one won't if the title isn't set to shb
            // it's set from some updateAnimation call somewhere in the setup process
            // we just manually set it here because I didn't want to figure that out
            if (ShouldModifyTitleScreenLogo && TitleScreenLogoOption == TitleScreenLogo.Shadowbringers)
            {
                SetTitleScreenLogoPartId();
            }
        }

        private AtkResNode* GetTitleScreenLogoResNode()
        {
            var addon = (AtkUnitBase*)Services.GameGui.GetAddonByName("_TitleLogo").Address;
            if (addon == null) return null;
            return addon->RootNode;
        }

        // force set TitleScreen logo part id for shb logo
        private void SetTitleScreenLogoPartId()
        {
            // Iterating through all nodes cause there's like 5 image nodes used in the animation for shadowbringers
            Utils.IterateNodes(GetTitleScreenLogoResNode(), node =>
            {
                if (node->Type == NodeType.Image)
                {
                    var imageNode = (AtkImageNode*)node;
                    imageNode->PartId = 1;
                }
            });
        }


        // Calls AtkTimeline::PlayAnimation
        private void AnimateDawntrailLogo()
        {
            var node = GetTitleScreenLogoResNode();
            if (node == null) return;
            Services.Log.Debug("Animating dawntrail logo");
            // Values taken by observing what the game calls, no clue what they mean :)
            node->Timeline->PlayAnimation(AtkTimelineJumpBehavior.LoopForever, (ushort)0x65);
        }

        // Advances AtkTimeline::FrameTime
        private void AdvanceTitleLogoAnimation()
        {
            var node = GetTitleScreenLogoResNode();
            if (node == null) return;
            Services.Log.Debug($"Advancing timeline animation by {advanceTitleLogoAnimationDuration}");
            // Just manually advance FrameTime to skip animation
            node->Timeline->FrameTime += advanceTitleLogoAnimationDuration;
        }

        // A recolerable addon's setup is complete, we change it's color here
        private void RecolorablePostSetup(AddonEvent type, AddonArgs args)
        {
            if (ShouldModifyTitleScreenUiColors)
            {
                RecolorAddon((AtkUnitBase*)args.Addon.Address);
            }
        }

        // Modify TextNodes and NineGrids (title menu button highlight)
        private void RecolorAddon(AtkUnitBase* addon)
        {
            if (addon != null && addon->RootNode != null)
            {
                Utils.IterateNodes(addon->RootNode, (node) =>
                {
                    if (node->Type == NodeType.Text)
                    {
                        Services.Log.Debug($"[RecolorablePostSetup] Recoloring text {(IntPtr)node:X} to {TitleScreenColorsOption.Color} {TitleScreenColorsOption.EdgeColor}");
                        var textNode = (AtkTextNode*)node;
                        textNode->TextColor = TitleScreenColorsOption.Color.ToByteColor();
                        textNode->EdgeColor = TitleScreenColorsOption.EdgeColor.ToByteColor();
                    }
                    else if (node->Type == NodeType.NineGrid)
                    {
                        var nineGridNode = (AtkNineGridNode*)node;
                        Services.Log.Debug($"[RecolorablePostSetup] Recoloring ninegrid {(IntPtr)node:X} to {TitleScreenColorsOption.HighlightColor}");
                        var color = TitleScreenColorsOption.HighlightColor - UiColors.HighlightColorApproximation;
                        nineGridNode->AddRed = nineGridNode->AddRed_2 = (short)(color.X * 255);
                        nineGridNode->AddGreen = nineGridNode->AddGreen_2 = (short)(color.Y * 255);
                        nineGridNode->AddBlue = nineGridNode->AddBlue_2 = (short)(color.Z * 255);
                        nineGridNode->Color.A = (byte)(color.W * 255);
                    }
                });
            }
        }

        // Selects whatever logo resource should be used
        // We manipulate LobbyInfo values to make sure the correct one gets picked and then restore them
        private bool PickTitleLogoDetour(AtkUnitBase* atkUnit)
        {
            shouldHideTitleLogoAddon = false;
            shouldAdvanceTitleLogoAnimation = false;
            shouldAnimateDawntrailLogo = false;
            // have to set it to false or some logos won't load because ???
            LobbyInfo->PreDawntrailLogoFlag = false;
            Services.Log.Debug($"[PickTitleLogoDetour] {(IntPtr)atkUnit:X} {atkUnit->IsVisible}");
            if (!ShouldModifyTitleScreenLogo)
            {
                return pickTitleLogoHook.Original(atkUnit);
            }

            bool currentFreeTrial = LobbyInfo->FreeTrial;
            TitleScreenExpansion currentTitleScreenType = LobbyInfo->CurrentTitleScreenType;
            if (TitleScreenLogoOption == TitleScreenLogo.None)
            {
                // Don't load anything at all
                return false;
            }
            else if (TitleScreenLogoOption == TitleScreenLogo.FreeTrial)
            {
                LobbyInfo->FreeTrial = true;
            }
            else
            {
                LobbyInfo->FreeTrial = false;
                LobbyInfo->CurrentTitleScreenType = TitleScreenLogoOption switch
                {
                    TitleScreenLogo.ARealmReborn => TitleScreenExpansion.ARealmReborn,
                    TitleScreenLogo.Heavensward => TitleScreenExpansion.Heavensward,
                    TitleScreenLogo.Stormblood => TitleScreenExpansion.Stormblood,
                    TitleScreenLogo.Shadowbringers => TitleScreenExpansion.Shadowbringers,
                    TitleScreenLogo.Endwalker => TitleScreenExpansion.Endwalker,
                    TitleScreenLogo.Dawntrail => TitleScreenExpansion.Dawntrail,
                    _ => LobbyInfo->CurrentTitleScreenType
                };
                if (!LobbyInfo->CurrentTitleScreenType.IsInAvailableExpansion())
                {
                    Services.Log.Warning($"[PickTitleLogoDetour] Tried to load missing {LobbyInfo->CurrentTitleScreenType.ToText()} logo");
                    Services.NotificationManager.AddNotification(new()
                    {
                        Content = $"Tried to load missing {LobbyInfo->CurrentTitleScreenType.ToText()} logo, adjust your settings or get the full game",
                        Title = "Missing files",
                        Type = NotificationType.Error,
                        Minimized = false
                    });
                    LobbyInfo->CurrentTitleScreenType = TitleScreenExpansion.ARealmReborn;
                }

                // Advance and handle animations if it doesn't match the TitleScreenOverride
                // Make an exception for dawntrial because we hide it until it animates and it won't animate by itself if the game is past that point in the cutscene (only relevant when flipping through options)
                if (titleScreenLocationModel.TitleScreenOverride != LobbyInfo->CurrentTitleScreenType || titleScreenLocationModel.TitleScreenOverride == TitleScreenExpansion.Dawntrail)
                {
                    if (LobbyInfo->CurrentTitleScreenType == TitleScreenExpansion.Stormblood)
                    {
                        shouldAdvanceTitleLogoAnimation = true;
                        advanceTitleLogoAnimationDuration = 1.2f;
                    }
                    else if (LobbyInfo->CurrentTitleScreenType == TitleScreenExpansion.Endwalker)
                    {
                        shouldAdvanceTitleLogoAnimation = true;
                        advanceTitleLogoAnimationDuration = 9.8f;
                    }
                    else if (LobbyInfo->CurrentTitleScreenType == TitleScreenExpansion.Dawntrail)
                    {
                        shouldAnimateDawntrailLogo = true;
                    }
                }
            }

            if (titleScreenLocationModel.TitleScreenOverride == TitleScreenExpansion.Dawntrail && !cutsceneAnimatedTitleLogo)
            {
                shouldHideTitleLogoAddon = true;
            }

            Services.Log.Debug($"[PickTitleLogoDetour] set {LobbyInfo->CurrentTitleScreenType} {LobbyInfo->FreeTrial}");
            bool result = pickTitleLogoHook.Original(atkUnit);
            LobbyInfo->FreeTrial = currentFreeTrial;
            LobbyInfo->CurrentTitleScreenType = currentTitleScreenType;
            return result;
        }

        // Attempt to reload just the Ui
        public void ReloadTitleScreenUi(bool force = false)
        {
            Services.Log.Debug($"[ReloadTitleScreenUi] Reloading title screen UI {CanReloadTitleScreen} {reloadingTitleScreen} {reloadingTitleScreenUi}");
            if ((CanReloadTitleScreen && !reloadingTitleScreenUi) || force)
            {
                if (reloadingTitleScreen) return; // if a full reload is in progress ignore this call
                reloadingTitleScreenUi = true;
                OnTitleMenuFinalize(ExecuteTitleScreenUiReload);
                removeTitleScreenUi(AgentLobby);
            }
        }

        // Recolor ui
        public void RecolorTitleScreenUi(bool force = false)
        {
            if (CanReloadTitleScreen || force)
            {
                foreach (var addon in recolorableAddons)
                {
                    RecolorAddon((AtkUnitBase*)Services.GameGui.GetAddonByName(addon).Address);
                }
            }
        }

        //Execute action if _TitleMenu is unloaded or when it cleans up
        private void OnTitleMenuFinalize(Action action)
        {
            if (Services.GameGui.GetAddonByName("_TitleMenu") != IntPtr.Zero)
            {
                var titleMenuAddon = (AtkUnitBase*)Services.GameGui.GetAddonByName("_TitleMenu").Address;
                Services.Log.Debug($"[OnTitleMenuFinalize] queueing {titleMenuAddon->IsVisible} {titleMenuAddon->IsReady} {titleMenuAddon->IsFullyLoaded()} {titleMenuAddon->Flags1A1:X} {titleMenuAddon->Flags1A2:X}");
                titleMenuFinalizeActions.Enqueue(action);
            }
            else
            {
                Services.Log.Debug("[OnTitleMenuFinalize] running instant");
                action();
            }
        }


        private void TitleMenuFinalize()
        {
            Services.Log.Debug("[TitleMenuFinalize]");
            foreach (var action in titleMenuFinalizeActions)
            {
                action();
            }

            titleMenuFinalizeActions.Clear();
        }

        // Reload UI by setting LobbyUiStage and letting the game handle it
        private void ExecuteTitleScreenUiReload()
        {
            Services.Log.Debug("[ExecuteTitleScreenUiReload]");
            LobbyUiStage = LobbyUiStage.LoadingTitleScreen2;
            reloadingTitleScreenUi = false;
        }

        private void DisposeUi()
        {
            Services.AddonLifecycle.UnregisterListener(RecolorablePostSetup);
            Services.AddonLifecycle.UnregisterListener(TitleLogoPostSetup);
            Services.AddonLifecycle.UnregisterListener(CharaSelectInfoUpdate);
            Services.AddonLifecycle.UnregisterListener(CharaSelectListMenuUpdate);
        }
    }
}
