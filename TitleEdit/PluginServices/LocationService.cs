using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TitleEdit.Data.Character;
using TitleEdit.Data.Layout;
using TitleEdit.Data.Persistence;
using TitleEdit.Extensions;
using TitleEdit.Utility;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.DrawDataContainer;
using Festival = TitleEdit.Data.Persistence.Festival;

namespace TitleEdit.PluginServices
{
    public class LocationService : AbstractService
    {
        private readonly static Regex FileNameRegex = new("^([A-F0-9]{16})\\.json$", RegexOptions.IgnoreCase);

        //Move this to a different service?
        public unsafe ushort TimeOffset
        {
            get
            {
                long etS = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->ClientTime.EorzeaTime;
                var et = DateTimeOffset.FromUnixTimeSeconds(etS);
                return (ushort)((et.Hour * 100) + et.Minute);
            }
        }

        private readonly CancellationTokenSource cancellationToken = new();
        private DateTime lastSave = DateTime.Now;
        private DateTime lastLocationCollection = DateTime.Now;
        public string? TerritoryPath { get; private set; }
        public IReadOnlyDictionary<ulong, LocationModel> Locations => locations;
        private ConcurrentDictionary<ulong, LocationModel> locations = [];
        private ulong lastContentId;
        private bool refreshLayout = true;
        private uint bgmId;
        private string? bgmPath = null;
        private bool isLoggedIn = false;

        private DirectoryInfo saveDirectory;

        private Task? saveTask;

        public Dictionary<uint, string> TerritoryPaths { get; private set; }
        public Dictionary<string, uint> TerritoryPathsReverse { get; private set; }

        public LocationService()
        {
            saveDirectory = ConfigurationService.GetBaseConfigDirectory().CreateSubdirectory(PersistanceConsts.CharacterLocationsFolder);
            TerritoryPaths = Services.DataManager.GetExcelSheet<TerritoryType>()!.ToDictionary(r => r.RowId, r => r.Bg.ToString());
            TerritoryPathsReverse = TerritoryPaths.GroupBy(r => r.Value).ToDictionary(r => r.Key, r => r.First().Key); // is it worth to load this into memory just for migration?
        }

        public override void LoadData()
        {
            LoadSavedLocations();
        }

        public override void Init()
        {
            Services.Framework.Update += Tick;
            Services.ClientState.Logout += Logout;
            Services.ClientState.TerritoryChanged += TerritoryChanged;
            Services.LayoutService.OnLayoutChange += LayoutChanged;
            Services.BgmService.OnBgmChange += BgmChanged;
            unsafe
            {
                Services.LayoutService.OnLayoutInstanceSetActive += LayoutInstanceSetActive;
                Services.LayoutService.OnVfxLayoutInstanceSetVfxTriggerIndex += VfxLayoutInstanceSetVfxTriggerIndex;
            }

            TerritoryChanged(Services.ClientState.TerritoryType);
            BgmChanged(Services.BgmService.CurrentSongId);
            saveTask = Task.Run(SaveTask, cancellationToken.Token);
        }

        private unsafe void VfxLayoutInstanceSetVfxTriggerIndex(VfxLayoutInstance* instance, int index)
        {
            Services.Log.Debug($"[VfxLayoutInstanceSetVfxTriggerIndex] {instance->ILayoutInstance.UUID():X} {index}");
            refreshLayout = true;
        }

        private unsafe void Tick(IFramework framework)
        {
            isLoggedIn = Services.ClientState.IsLoggedIn;
            if (!Services.ConfigurationService.TrackPlayerLocation || (DateTime.Now - lastLocationCollection).TotalSeconds < 2) return;
            if (Services.ObjectTable.LocalPlayer != null)
            {
                lastContentId = Services.PlayerState.ContentId;
                if (TerritoryPath != null)
                {
                    var locationModel = locations.TryGetValue(lastContentId, out var location) ? location : new();
                    locationModel.TerritoryTypeId = Services.ClientState.TerritoryType;
                    locationModel.LayoutTerritoryTypeId = Services.LayoutService.LayoutTerritoryId;
                    locationModel.LayoutLayerFilterKey = Services.LayoutService.LayoutLayerFilterKey;
                    locationModel.TerritoryPath = TerritoryPath;
                    locationModel.Position = Services.ObjectTable.LocalPlayer.Position;
                    locationModel.Rotation = Services.ObjectTable.LocalPlayer.Rotation;
                    locationModel.WeatherId = Services.WeatherService.WeatherId;
                    var character = ((CharacterExpanded*)Services.ObjectTable.LocalPlayer.Address);
                    locationModel.MovementMode = character->MovementMode;
                    if (Services.ConfigurationService.SaveMount)
                    {
                        SetMount(ref locationModel, &character->Character);
                    }

                    if (Services.ConfigurationService.SaveTime)
                    {
                        locationModel.TimeOffset = TimeOffset;
                    }
                    else
                    {
                        locationModel.UseLiveTime = true;
                    }

                    if (Services.ConfigurationService.SaveBgm)
                    {
                        locationModel.BgmPath = bgmPath;
                        locationModel.BgmId = bgmId;
                    }
                    else
                    {
                        locationModel.BgmId = 0;
                        locationModel.BgmPath = null;
                    }

                    if (Services.ConfigurationService.SaveLayout && (!Services.Condition.Any(ConditionFlag.BoundByDuty) || Services.ConfigurationService.SaveLayoutInInstance))
                    {
                        if (Services.LayoutService.LayoutInitialized)
                        {
                            if (refreshLayout)
                            {
                                SetLayout(ref locationModel);
                            }

                            refreshLayout = false;
                        }

                        locationModel.SaveLayout = true;
                        locationModel.SaveFestivals = true;
                        locationModel.SaveHousing = Services.ConfigurationService.SaveHousing;
                    }
                    else
                    {
                        locationModel.SaveLayout = false;
                        locationModel.SaveHousing = false;
                        locationModel.SaveFestivals = false;
                        locationModel.UseVfx = false;
                        locationModel.Active.Clear();
                        locationModel.Inactive.Clear();
                        locationModel.VfxTriggerIndexes.Clear();
                        locationModel.Festivals = [];
                        locationModel.Furniture = null;
                        locationModel.Plots = null;
                        locationModel.Estate = null;
                    }

                    locations[lastContentId] = locationModel;
                }
                else
                {
                    Services.Log.Debug("No territory path?");
                }
            }

            lastLocationCollection = DateTime.Now;
        }

        public unsafe void SetMount(ref LocationModel locationModel, Character* character)
        {
            locationModel.Mount.MountId = character->Mount.MountId;
            if (character->Mount.MountId != 0)
            {
                var mountCharacter = character->Mount.MountObject;
                locationModel.Mount.BuddyModelTop = (uint)mountCharacter->DrawData.Equipment(EquipmentSlot.Head).Value;
                locationModel.Mount.BuddyModelBody = (uint)mountCharacter->DrawData.Equipment(EquipmentSlot.Body).Value;
                locationModel.Mount.BuddyModelLegs = (uint)mountCharacter->DrawData.Equipment(EquipmentSlot.Feet).Value;
                locationModel.Mount.BuddyStain = mountCharacter->DrawData.Equipment(EquipmentSlot.Legs).Stain0;
            }
        }

        private void TerritoryChanged(ushort territoryId)
        {
            TerritoryPath = TerritoryPaths.GetValueOrDefault(territoryId);
            Services.Log.Debug($"TerritoryChanged: {TerritoryPath}");
        }

        public void Logout(int type, int code)
        {
            Save(lastContentId);
        }

        private void LayoutChanged()
        {
            refreshLayout = true;
        }

        private void BgmChanged(uint songId)
        {
            bgmId = songId;
            bgmPath = Services.BgmService.BgmPaths[bgmId];
            Services.Log.Debug($"BgmChanged {songId} {bgmPath}");
        }

        //Hook on whatever deletes stuff (and maybe adds) so we can get rid of the constant refreshing cause in solution 9 it might cause lag spikes
        private unsafe void LayoutInstanceSetActive(ILayoutInstance* layout, byte active)
        {
            if (Services.ConfigurationService.SaveLayout && !refreshLayout && Services.PlayerState.IsLoaded && locations.TryGetValue(Services.PlayerState.ContentId, out var locationModel))
            {
                var inActive = locationModel.Active.Contains(layout->UUID());
                var inInactive = locationModel.Inactive.Contains(layout->UUID());
                if ((inActive || inInactive) && ((active != 0 && !inActive) || (active == 0 && !inInactive)))
                {
                    refreshLayout = true;
                    Services.Log.Debug($"[LayoutSetActiveDetour] refreshLayout");
                }
            }
        }

        public unsafe void SetLayout(ref LocationModel locationModel)
        {
            Services.Log.Debug("SetLayout called");
            HashSet<ulong> active = [];
            HashSet<ulong> inactive = [];
            Dictionary<ulong, short> vfxTriggerIndexes = [];
            Services.LayoutService.ForEachInstance(instance =>
            {
                if (instance.Value->IsActive())
                {
                    active.Add(instance.Value->UUID());
                }
                else
                {
                    inactive.Add(instance.Value->UUID());
                }

                if (instance.Value->Id.Type == InstanceType.Vfx)
                {
                    var vfxInstance = (VfxLayoutInstance*)instance.Value;
                    if (vfxInstance->VfxTriggerIndex != -1)
                    {
                        vfxTriggerIndexes[instance.Value->UUID()] = vfxInstance->VfxTriggerIndex;
                    }
                }
            });
            locationModel.Active = active;
            locationModel.Inactive = inactive;
            locationModel.VfxTriggerIndexes = vfxTriggerIndexes;
            locationModel.Festivals = Services.LayoutService.LayoutManager->ActiveFestivals
                                      .ToArray()
                                      .Select(item => new Festival(item))
                                      .ToArray();

            if (locationModel is { SaveLayout: true, SaveHousing: true })
            {
                Services.HousingService.SetHousing(ref locationModel);
            }
        }

        public void Save(ulong localContentId)
        {
            //Services.Log.Debug($"Save {localContentId:X16}");
            //var sw = Stopwatch.StartNew();
            try
            {
                if (locations.TryGetValue(localContentId, out LocationModel locationModel))
                {
                    FilesystemUtil.WriteAllTextSafe(
                        Path.Join(saveDirectory.FullName, $"{localContentId:X16}.json"),
                        JsonConvert.SerializeObject(locationModel)
                    );
                }
                else
                {
                    Services.Log.Warning("Failed to save contentId is null");
                }
            }
            catch (Exception e)
            {
                Services.Log.Error(e, e.Message);
            }
            //sw.Stop();

            //Services.Log.Debug($"Save took {sw.Elapsed.TotalMilliseconds} ms");
        }

        private async Task SaveTask()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if ((DateTime.Now - lastSave).TotalSeconds > Services.ConfigurationService.SavePeriod)
                {
                    if (Services.ConfigurationService.TrackPlayerLocation && Services.ConfigurationService.PeriodicSaving && isLoggedIn)
                    {
                        Save(lastContentId);
                    }

                    lastSave = DateTime.Now;
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken.Token);
            }
        }

        private void LoadSavedLocations()
        {
            foreach (var file in saveDirectory.EnumerateFiles())
            {
                var match = FileNameRegex.Match(file.Name);
                if (match.Success)
                {
                    ulong contentId = Convert.ToUInt64(match.Groups[1].Value, 16);
                    Services.Log.Debug($"Loading {contentId:X16}");
                    try
                    {
                        var location = Services.MigrationService.MigrateLocation(File.ReadAllText(file.FullName));
                        Validate(location);
                        locations[contentId] = location;
                    }
                    catch (Exception e)
                    {
                        Services.Log.Error(e, e.Message);
                    }
                }
                else
                {
                    Services.Log.Debug($"Unknown file in config directory {file.Name}");
                }
            }
        }

        public LocationModel GetLocationModel(ulong contentId)
        {
            if (locations.TryGetValue(contentId, out var locationModel))
            {
                return locationModel;
            }

            return Services.PresetService.GetDefaultPreset(LocationType.CharacterSelect).LocationModel;
        }

        public void Validate(LocationModel locationModel)
        {
            if (locationModel.Version != LocationModel.CURRENT_VERSION)
            {
                throw new($"Location Version is not valid {LocationModel.CURRENT_VERSION}");
            }

            if (locationModel.TitleScreenOverride == null)
            {
                if (!Services.DataManager.FileExists($"bg/{locationModel.TerritoryPath}.lvb"))
                {
                    throw new($"Game scene file '{locationModel.TerritoryPath}' not found");
                }

                if (!locationModel.BgmPath.IsNullOrEmpty() && !Services.DataManager.FileExists(locationModel.BgmPath))
                {
                    throw new($"BGM file '{locationModel.BgmPath}' not found");
                }

                if (locationModel is { Estate: not null, Plots: not null })
                {
                    throw new($"Location has both plot and estate data defined");
                }

                if (locationModel.Festivals != null && locationModel.Festivals.Length != LocationModel.FESTIVAL_COUNT)
                {
                    throw new($"Location has an invalid festival count {locationModel.Festivals.Length}, should be {LocationModel.FESTIVAL_COUNT}");
                }
            }
            else if (!locationModel.TitleScreenOverride.IsInAvailableExpansion())
            {
                throw new($"Expansion '{locationModel.TitleScreenOverride}' is not available");
            }
        }

        public void LayoutSettingsUpdated()
        {
            refreshLayout = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            Services.Framework.Update -= Tick;
            Services.ClientState.Logout -= Logout;
            Services.ClientState.TerritoryChanged -= TerritoryChanged;
            Services.LayoutService.OnLayoutChange -= LayoutChanged;
            Services.BgmService.OnBgmChange -= BgmChanged;
            unsafe
            {
                Services.LayoutService.OnLayoutInstanceSetActive -= LayoutInstanceSetActive;
            }

            cancellationToken.Cancel();
            try
            {
                saveTask?.Wait(-1, cancellationToken.Token);
            }
            catch (OperationCanceledException) { } finally
            {
                cancellationToken.Dispose();
            }
        }
    }
}
