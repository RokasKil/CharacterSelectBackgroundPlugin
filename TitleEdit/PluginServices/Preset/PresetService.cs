using Dalamud.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Preset
{
    public partial class PresetService : AbstractService
    {
        private readonly static Regex FileInvalidSymbolsRegex = new(@"[/\\:*?|""<>]");

        public IReadOnlyDictionary<string, PresetModel> Presets => presets;

        public IEnumerable<KeyValuePair<string, PresetModel>> CharacterSelectPresetEnumerator => Presets.Where(preset => preset.Value.LocationModel.LocationType == LocationType.CharacterSelect);

        public IEnumerable<KeyValuePair<string, PresetModel>> TitleScreenPresetEnumerator => Presets.Where(preset => preset.Value.LocationModel.LocationType == LocationType.TitleScreen);

        public IEnumerable<KeyValuePair<string, PresetModel>> EditablePresetEnumerator => Presets.Where(preset => !preset.Key.StartsWith("?"));

        private readonly Dictionary<string, PresetModel> presets = new(StringComparer.InvariantCultureIgnoreCase);

        private readonly DirectoryInfo saveDirectory;

        public PresetService()
        {
            saveDirectory = ConfigurationService.GetBaseConfigDirectory().CreateSubdirectory(PersistanceConsts.PresetsFolder);
        }

        public override void LoadData()
        {
            LoadBasePresets();
            LoadSavedPresets();
        }

        private void LoadSavedPresets()
        {
            Utils.IterateFiles(saveDirectory, (file, relativePath) =>
            {
                var relativeFilePath = Path.Join(relativePath, file.Name);
                if (file.Name.EndsWith(".json", true, null))
                {
                    Services.Log.Debug($"Loading {relativeFilePath}");
                    try
                    {
                        var preset = Load(file.FullName, relativePath: relativePath);
                        presets[preset.FileName] = preset;
                    }
                    catch (Exception e)
                    {
                        Services.Log.Error(e, e.Message);
                    }
                }
                else
                {
                    Services.Log.Debug($"Unknown file in preset directory {relativeFilePath}");
                }
            });
        }

        public string Save(PresetModel preset)
        {
            Validate(preset);
            if (string.IsNullOrEmpty(preset.FileName))
            {
                var namePart = FileInvalidSymbolsRegex.Replace(preset.Name, "").Truncate(50);
                if (presets.ContainsKey($"{namePart}.json"))
                {
                    var i = 1;
                    while (presets.ContainsKey($"{namePart} ({i}).json")) i++;
                    namePart = $"{namePart} ({i})";
                }

                preset.FileName = $"{namePart}.json";
            }

            try
            {
                Services.Log.Debug($"Saving {preset.FileName}");
                FilesystemUtil.WriteAllTextSafe(
                    Path.Join(saveDirectory.FullName, preset.FileName),
                    JsonConvert.SerializeObject(preset)
                );
            }
            catch (Exception e)
            {
                Services.Log.Error(e, e.Message);
                throw;
            }

            presets[preset.FileName] = preset;
            return preset.FileName;
        }

        public void Export(string presetFileName, string filePath)
        {
            if (presets.TryGetValue(presetFileName, out var preset))
            {
                try
                {
                    Services.Log.Debug($"Exporting {preset.FileName} to {filePath}");
                    FilesystemUtil.WriteAllTextSafe(
                        filePath,
                        JsonConvert.SerializeObject(preset)
                    );
                }
                catch (Exception e)
                {
                    Services.Log.Error(e, e.Message);
                    throw;
                }
            }
            else
            {
                throw new("Preset not found");
            }
        }

        public string ExportText(string presetFileName)
        {
            if (presets.TryGetValue(presetFileName, out var preset))
            {
                try
                {
                    Services.Log.Debug($"Exporting {preset.FileName}");
                    return "TE3" + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(preset)));
                }
                catch (Exception e)
                {
                    Services.Log.Error(e, e.Message);
                    throw;
                }
            }

            throw new("Preset not found");
        }

        public string Import(string filePath)
        {
            PresetModel preset;
            try
            {
                preset = Load(filePath, false);
            }
            catch (Exception e)
            {
                Services.Log.Error(e, e.Message);
                throw;
            }

            return Save(preset);
        }

        public async Task<string> ImportText(string textData)
        {
            PresetModel preset;
            try
            {
                if (textData.StartsWith("TE2") || textData.StartsWith("TE3"))
                {
                    textData = Encoding.UTF8.GetString(Convert.FromBase64String(textData[3..]));
                }

                var shareCode = Services.ShareService.GetCodeFromShareUrl(textData);
                // Check if it is a share link and download it
                if (shareCode != null)
                {
                    preset = await Services.ShareService.GetPreset(shareCode);
                }
                else
                {
                    preset = LoadText(textData);
                }
            }
            catch (Exception e)
            {
                Services.Log.Error(e, e.Message);
                throw;
            }

            return Save(preset);
        }

        private PresetModel Load(string path, bool setFileName = true, string relativePath = "")
        {
            var file = new FileInfo(path);
            var preset = LoadText(File.ReadAllText(file.FullName));
            if (setFileName)
            {
                preset.FileName = Path.Join(relativePath, file.Name);
            }

            return preset;
        }

        public PresetModel LoadText(string textData)
        {
            var preset = Services.MigrationService.MigratePreset(textData);
            Validate(preset);
            return preset!.Value;
        }

        public void Validate(PresetModel? presetOpt)
        {
            if (!presetOpt.HasValue)
            {
                throw new("Invalid preset");
            }

            var preset = presetOpt.Value;
            if (preset.Version != PresetModel.CURRENT_VERSION)
            {
                throw new($"Preset Version is not valid {preset.Version}");
            }

            if (string.IsNullOrEmpty(preset.Name))
            {
                throw new("Preset doesn't have a name");
            }

            Services.LocationService.Validate(preset.LocationModel);
        }

        public void Delete(string presetFileName)
        {
            if (presets.ContainsKey(presetFileName))
            {
                File.Delete(Path.Join(saveDirectory.FullName, presetFileName));
                presets.Remove(presetFileName);
            }

            return;
        }

        public bool TryGetPreset(string presetFileName, out PresetModel preset, LocationType? type = null)
        {
            if (presets.TryGetValue(presetFileName, out preset) && (type == null || preset.LocationModel.LocationType == type))
            {
                return true;
            }

            return false;
        }


        public PresetModel GetDefaultPreset(LocationType type)
        {
            return type switch
            {
                LocationType.CharacterSelect => presets["?/AetherialSea.json"],
                LocationType.TitleScreen => presets["?/ARealmReborn.json"],
                _ => throw new() // should never hit
            };
        }
    }
}
