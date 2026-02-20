using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TitleEdit.Data.Persistence;
using TitleEdit.Data.Persistence.Migration;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Migration
{
    public partial class MigrationService
    {
        public static readonly string[] IncludedPresets = ["A Realm Reborn", "Dawntrail", "Endwalker", "Heavensward", "Shadowbringers", "Stormblood"];
        public PresetModel? MigratePreset(string presetTextData) => MigratePreset(presetTextData, out _);

        public PresetModel? MigratePreset(string presetTextData, out bool changed)
        {
            changed = false;
            PresetModel? preset = JsonConvert.DeserializeObject<PresetModel>(presetTextData);
            if (preset?.Version == null)
            {
                preset = MigrateTitleScreenV2(presetTextData);
                if (preset == null)
                {
                    changed = true;
                    return null;
                }
            }

            changed = true;
            switch (preset.Value.Version)
            {
                case 1:
                    preset = MigrateV1(preset.Value);
                    goto case 2;
                case 2:
                    preset = MigrateV2(preset.Value);
                    goto case 3;
                case 3:
                    preset = MigrateV3(preset.Value);
                    goto case 4;
                case 4:
                    preset = MigrateV4(preset.Value);
                    goto case 5;
                case 5:
                    preset = MigrateV5(preset.Value);
                    break;
                default:
                    break;
            }

            return preset;
        }

        private PresetModel? MigrateTitleScreenV2(string presetTextData)
        {
            Services.Log.Info($"Migrating TitleScreenV2 preset");
            TitleEditV2Screen oldPreset;
            try
            {
                oldPreset = JsonConvert.DeserializeObject<TitleEditV2Screen>(presetTextData);
            }
            catch (Exception e)
            {
                Services.Log.Error(e, $"Failed to read TitleScreenV2 preset: {Convert.ToBase64String(Encoding.UTF8.GetBytes(presetTextData))}");
                return null;
            }

            if (oldPreset.Name != null &&
                oldPreset.Logo != null &&
                oldPreset.TerritoryPath != null &&
                oldPreset.CameraPos != null &&
                oldPreset.FixOnPos != null &&
                oldPreset.FovY != null &&
                oldPreset.WeatherId != null &&
                oldPreset.BgmPath != null)
            {
                PresetModel preset = new();
                preset.Name = oldPreset.Name;
                if (oldPreset.DisplayLogo == null || !oldPreset.DisplayLogo.Value)
                {
                    preset.LocationModel.TitleScreenLogo = TitleScreenLogo.None;
                }
                else
                {
                    preset.LocationModel.TitleScreenLogo = oldPreset.Logo switch
                    {
                        "A Realm Reborn" => TitleScreenLogo.ARealmReborn,
                        "FFXIV Free Trial" => TitleScreenLogo.FreeTrial,
                        "Heavensward" => TitleScreenLogo.Heavensward,
                        "Stormblood" => TitleScreenLogo.Stormblood,
                        "Shadowbringers" => TitleScreenLogo.Shadowbringers,
                        "Endwalker" => TitleScreenLogo.Endwalker,
                        "Dawntrail" => TitleScreenLogo.Dawntrail,
                        "Unspecified" => TitleScreenLogo.Unspecified,
                        _ => TitleScreenLogo.None
                    };
                }

                preset.LocationModel.TerritoryPath = oldPreset.TerritoryPath;
                preset.LocationModel.TerritoryTypeId = (ushort)Services.LocationService.TerritoryPathsReverse.GetValueOrDefault(oldPreset.TerritoryPath);
                preset.LocationModel.Position = oldPreset.CameraPos.Value;
                preset.LocationModel.CameraPosition = oldPreset.CameraPos.Value;
                (preset.LocationModel.Yaw, preset.LocationModel.Pitch) = Utils.GetAnglesFromVector(oldPreset.FixOnPos.Value - oldPreset.CameraPos.Value);
                preset.LocationModel.Roll = 0;
                preset.LocationModel.Fov = oldPreset.FovY.Value;
                preset.LocationModel.WeatherId = oldPreset.WeatherId.Value;
                preset.LocationModel.TimeOffset = oldPreset.TimeOffset.GetValueOrDefault();
                preset.LocationModel.BgmPath = oldPreset.BgmPath;
                preset.LocationModel.BgmId = Services.BgmService.BgmPathsReverse.GetValueOrDefault(oldPreset.BgmPath);
                preset.LocationModel.TitleScreenOverride = oldPreset.TitleOverride;
                preset.LocationModel.SaveLayout = false;
                preset.LocationModel.UseVfx = true;
                preset.LocationModel.SaveFestivals = false;

                // Fail if we can't find TerritoryTypeId or BgmId?
                return preset;
            }

            Services.Log.Info($"Failed to migrate TitleScreenV2 preset");
            return null;
        }

        private PresetModel MigrateV1(PresetModel preset)
        {
            Services.Log.Info($"Migrating preset to v2 {preset.Name}");
            preset.Version = 2;
            preset.LocationModel = MigrateV1(preset.LocationModel);
            return preset;
        }

        private PresetModel MigrateV2(PresetModel preset)
        {
            Services.Log.Info($"Migrating preset to v3 {preset.Name}");
            preset.Version = 3;
            preset.LocationModel = MigrateV2(preset.LocationModel);
            return preset;
        }

        private PresetModel MigrateV3(PresetModel preset)
        {
            Services.Log.Info($"Migrating preset to v4 {preset.Name}");
            preset.Version = 4;
            preset.LocationModel = MigrateV3(preset.LocationModel);

#pragma warning disable CS0612 // Type or member is obsolete
            if (preset.LastLocationMount)
            {
                preset.LocationModel.Mount = new() { LastLocationMount = true };
            }
#pragma warning restore CS0612 // Type or member is obsolete
            return preset;
        }

        private PresetModel MigrateV4(PresetModel preset)
        {
            Services.Log.Info($"Migrating preset to v5 {preset.Name}");
            preset.Version = 5;
            preset.LocationModel = MigrateV4(preset.LocationModel);
            return preset;
        }

        private PresetModel MigrateV5(PresetModel preset)
        {
            Services.Log.Info($"Migrating preset to v6 {preset.Name}");
            preset.Version = 6;
            preset.LocationModel = MigrateV5(preset.LocationModel);
            return preset;
        }

        public int MigrateTitleScreenV2Presets()
        {
            int migrated = 0;
            Services.PluginInterface.ConfigDirectory.Create();
            foreach (var file in Services.PluginInterface.ConfigDirectory.EnumerateFiles())
            {
                if (file.Extension.Equals(".json", System.StringComparison.OrdinalIgnoreCase) &&
                    // Don't migrate old included presets
                    !file.Name.StartsWith("TE_") &&
                    !IncludedPresets.Contains(file.Name[..^5]))
                {
                    Services.Log.Info($"Attempting to migrate {file.Name}");
                    var presetOpt = MigrateTitleScreenV2(File.ReadAllText(file.FullName));
                    if (presetOpt.HasValue)
                    {
                        var preset = presetOpt.Value;
                        preset.FileName = file.Name[..^5] + ".json";
                        Services.PresetService.Save(preset);
                        migrated++;
                    }
                }
                else
                {
                    Services.Log.Info($"Ignoring file {file.Name}");
                }
            }

            return migrated;
        }
    }
}
