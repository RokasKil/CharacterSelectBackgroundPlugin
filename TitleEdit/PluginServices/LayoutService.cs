using Dalamud.Game.ClientState.Conditions;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Group;
using FFXIVClientStructs.Interop;
using System;
using System.Collections.Generic;
#if CALC_LAYOUT_UPDATE
using System.Diagnostics;
#endif
using TitleEdit.Data.Layout;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices
{
    // This needs rewriting/optimizing
    public class LayoutService : AbstractService
    {
        public unsafe delegate void LayoutInstanceSetActiveDelegate(ILayoutInstance* instance, byte active);

        public unsafe delegate void VfxLayoutInstanceSetVfxTriggerIndexDelegate(VfxLayoutInstance* vfxInstance, int index);

        public delegate void LayoutWorldInitManagerDelegate(nint layoutWorld, uint p2, nint territoryPath, uint territoryId, uint layerFilterKey, nint festivals, uint contentFinderConditionId);

        private Hook<VfxLayoutInstanceSetVfxTriggerIndexDelegate> vfxLayoutInstanceSetVfxTriggerIndexHook = null!;
        private Hook<LayoutWorldInitManagerDelegate> layoutWorldInitManagerHook = null!;

        public unsafe event LayoutInstanceSetActiveDelegate? OnLayoutInstanceSetActive;
        public unsafe event VfxLayoutInstanceSetVfxTriggerIndexDelegate? OnVfxLayoutInstanceSetVfxTriggerIndex;
        public event Action? OnLayoutChange;

        public unsafe LayoutManager* LayoutManager => LayoutWorld.Instance()->ActiveLayout;
        public unsafe bool LayoutInitialized => LayoutManager->InitState == 7;
        public unsafe uint LayoutTerritoryId => LayoutManager->TerritoryTypeId;
        public unsafe uint LayoutLayerFilterKey => LayoutManager->LayerFilterKey;
        public Dictionary<IntPtr, Hook<LayoutInstanceSetActiveDelegate>> ActiveHooks { get; set; } = [];

        private bool ShouldEnableHooks => Services.ConfigurationService.TrackPlayerLocation &&
                                          Services.ConfigurationService.SaveLayout &&
                                          (!Services.Condition.Any(ConditionFlag.BoundByDuty) || Services.ConfigurationService.SaveLayoutInInstance);

        private readonly List<InstanceType> instanceTypes =
        [
            InstanceType.BgPart,
            InstanceType.Light,
            InstanceType.Vfx,
            InstanceType.SharedGroup,
            InstanceType.Decal
        ];
#if CALC_LAYOUT_UPDATE
        private Stopwatch updateSW = new();
#endif
        public double UpdateTime = 0;

        public LayoutService()
        {
            Services.GameInteropProvider.InitializeFromAttributes(this);
        }

        public override void Init()
        {
            unsafe
            {
                vfxLayoutInstanceSetVfxTriggerIndexHook = Hook<VfxLayoutInstanceSetVfxTriggerIndexDelegate>("48 89 5C 24 ?? 57 48 83 EC ?? 8B FA 48 8B D9 83 FA ?? 75", SetVfxLayoutInstanceVfxTriggerIndexDetour);
                layoutWorldInitManagerHook = Hook<LayoutWorldInitManagerDelegate>("E8 ?? ?? ?? ?? 8B 4C 24 ?? 33 DB", LayoutWorldInitManagerDetour);
            }

#if CALC_LAYOUT_UPDATE
            Services.Framework.Update += Tick;
#endif
            TerritoryChanged();
            EnableHooks();
            HookLayoutInstance("48 8D 0D ?? ?? ?? ?? 66 90 48 89 50"); // BgPart
            HookLayoutInstance("48 8D 0D ?? ?? ?? ?? 4C 89 70 ?? 44 89 70"); // Light
            HookLayoutInstance("48 8D 05 ?? ?? ?? ?? 48 89 01 33 D2 48 89 51 ?? 48 89 51"); // Vfx
            HookLayoutInstance("48 8D 05 ?? ?? ?? ?? 48 89 07 48 8D 05 ?? ?? ?? ?? 48 89 47 ?? 48 8D 05 ?? ?? ?? ?? 48 89 77"); // SharedGroup
        }

        private unsafe void HookLayoutInstance(string signature, int offset = 0)
        {
            // hooking 63rd virtual function 
            var address = Services.SigScanner.GetStaticAddressFromSig(signature, offset) + (63 * 8);
            //Services.Log.Debug($"Hooking {signature} at 0x{address:X16}");
            if (ActiveHooks.ContainsKey(address)) return;
            var hook = HookFromFunctionPointerVariable<LayoutInstanceSetActiveDelegate>(address, (layout, active) => LayoutInstanceSetActiveDetour(address, layout, active));
            ActiveHooks[address] = hook;
            if (ShouldEnableHooks)
            {
                hook.Enable();
            }
        }

        private unsafe void LayoutWorldInitManagerDetour(nint layoutWorld, uint p2, nint territoryPath, uint territoryId, uint layerFilterKey, nint festivals, uint contentFinderConditionId)
        {
            layoutWorldInitManagerHook.Original(layoutWorld, p2, territoryPath, territoryId, layerFilterKey, festivals, contentFinderConditionId);
            TerritoryChanged();
        }

        private unsafe void SetVfxLayoutInstanceVfxTriggerIndexDetour(VfxLayoutInstance* vfxInstance, int index)
        {
            vfxLayoutInstanceSetVfxTriggerIndexHook?.Original(vfxInstance, index);
            OnVfxLayoutInstanceSetVfxTriggerIndex?.Invoke(vfxInstance, index);
        }

        private void Tick(IFramework framework)
        {
#if CALC_LAYOUT_UPDATE
            UpdateTime = updateSW.Elapsed.TotalMilliseconds;
            updateSW.Reset();
#endif
        }

        private void TerritoryChanged()
        {
            Services.Log.Debug($"[TerritoryChanged]");
            OnLayoutChange?.Invoke();
        }

        private unsafe void LayoutInstanceSetActiveDetour(IntPtr funcAddress, ILayoutInstance* instance, byte active)
        {
            if (instance->Layout == LayoutManager)
            {
                // SetActive might be outside of the main thread
                Services.Framework.RunOnFrameworkThread(() =>
                {
#if CALC_LAYOUT_UPDATE
                    updateSW.Start();
#endif
                    OnLayoutInstanceSetActive?.Invoke(instance, active);

#if CALC_LAYOUT_UPDATE
                    updateSW.Stop();
#endif
                });
            }

            ActiveHooks[funcAddress].Original(instance, active);
        }


        public unsafe void ForEachInstance(Action<Pointer<ILayoutInstance>> action)
        {
            ForEachInstance(LayoutManager, action);
        }

        public unsafe void ForEachInstance(LayoutManager* manager, Action<Pointer<ILayoutInstance>> action)
        {
            foreach (var entry in manager->Layers)
            {
                var layer = entry.Item2.Value;
                foreach (var instanceEntry in layer->Instances)
                {
                    ForEachInstanceAndDescendants(instanceEntry.Item2, action);
                }
            }
        }

        public unsafe void ForEachInstanceAndDescendants(ILayoutInstance* instance, Action<Pointer<ILayoutInstance>> action)
        {
            if (instanceTypes.Contains(instance->Id.Type))
            {
                action(instance);
                if (instance->Id.Type == InstanceType.SharedGroup)
                {
                    var prefabInstance = (SharedGroupLayoutInstance*)instance;
                    foreach (var instanceData in prefabInstance->Instances.Instances.AsSpan())
                    {
                        ForEachInstanceAndDescendants(instanceData.Value->Instance, action);
                    }
                }
            }
        }

        public unsafe void SetVfxLayoutInstanceVfxTriggerIndex(VfxLayoutInstance* instance, int index) => vfxLayoutInstanceSetVfxTriggerIndexHook.Original(instance, index);


        public void SettingsUpdated()
        {
            foreach (var hook in ActiveHooks.Values)
            {
                if (ShouldEnableHooks)
                {
                    hook.Enable();
                }
                else
                {
                    hook.Disable();
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
#if CALC_LAYOUT_UPDATE
            Services.Framework.Update -= Tick;
#endif
        }
    }
}