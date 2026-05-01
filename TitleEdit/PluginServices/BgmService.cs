using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TitleEdit.Data.Bgm;
using TitleEdit.Data.BGM;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices
{
    //Taken from https://github.com/lmcintyre/OrchestrionPlugin/blob/main/Orchestrion/BgmSystem
    // and https://github.com/lmcintyre/TitleEditPlugin/blob/main/TitleEdit/BgmSheetManager.cs
    // and slightly modified of course
    public class BgmService : AbstractService
    {
        private const string SheetPath = @"https://docs.google.com/spreadsheets/d/1s-xJjxqp6pwS7oewNy1aOQnr3gaJbewvIBbyYchZ6No/gviz/tq?tqx=out:csv&sheet={0}";
        private const string SheetFileName = "xiv_bgm_{0}.csv";
        private readonly HttpClient client = new();

        private nint baseAddress;
        private const int SceneCount = 12;

        public nint BgmSceneManager
        {
            get
            {
                var baseObject = Marshal.ReadIntPtr(baseAddress);

                return baseObject;
            }
        }

        public nint BgmSceneList
        {
            get
            {
                var baseObject = Marshal.ReadIntPtr(baseAddress);

                // I've never seen this happen, but the game checks for it in a number of places
                return baseObject == nint.Zero ? nint.Zero : Marshal.ReadIntPtr(baseObject + 0xC0);
            }
        }


        public uint CurrentSongId { get; private set; }


        public Dictionary<uint, BgmInfo> Bgms { get; private set; }
        public Dictionary<uint, string> BgmPaths { get; private set; }
        public Dictionary<string, uint> BgmPathsReverse { get; private set; }

        public delegate void BgmChangedDelegate(uint songId);

        public unsafe event BgmChangedDelegate? OnBgmChange;

        private readonly CancellationTokenSource disposeCts = new();

        public BgmService()
        {
            baseAddress = Services.SigScanner.GetStaticAddressFromSig("48 8B 1D ?? ?? ?? ?? 8B F1");
            Bgms = [];
            BgmPaths = Services.DataManager.GetExcelSheet<BGM>()!.ToDictionary(r => r.RowId, r => r.File.ToString());
            BgmPathsReverse = BgmPaths.GroupBy(r => r.Value).ToDictionary(r => r.Key, r => r.First().Key); // is it worth to load this into memory just for migration?
        }

        public override void LoadData()
        {
            Services.Log.Information("[SongList] Loading local version");
            bool failedToLoadLocal = false;
            try
            {
                LoadLangSheet(GetLocalSheet("en"), "en");
            }
            catch (Exception e)
            {
                failedToLoadLocal = true;
                Services.Log.Error(e, "[SongList] Failed to load local version");
            }

            Task loadRemoteTask = LoadRemoteData(disposeCts.Token);
            if (failedToLoadLocal)
            {
                loadRemoteTask.GetAwaiter().GetResult();
            }
            else
            {
                loadRemoteTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Services.Log.Error(t.Exception, "[SongList] Failed to load remote version");
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public override void Init()
        {
            Services.Framework.Update += Tick;
        }

        private async Task LoadRemoteData(CancellationToken cancellationToken)
        {
            Services.Log.Information("[SongList] Checking for updated bgm sheets");
            string sheetData = await GetRemoteSheet("en", cancellationToken).ConfigureAwait(false);
            await Services.Framework.Run(() => LoadLangSheet(sheetData, "en"), cancellationToken).ConfigureAwait(false);
            Services.Log.Information("[SongList] Loaded remote version");
        }

        private async Task<string> GetRemoteSheet(string code, CancellationToken cancellationToken)
        {
            return await client.GetStringAsync(string.Format(SheetPath, code), cancellationToken);
        }

        private string GetLocalSheet(string code)
        {
            return File.ReadAllText(Path.Combine(Services.PluginInterface.AssemblyLocation.DirectoryName!, string.Format(SheetFileName, code)));
        }

        private void SaveLocalSheet(string text, string code)
        {
            FilesystemUtil.WriteAllTextSafe(Path.Combine(Services.PluginInterface.AssemblyLocation.DirectoryName!, string.Format(SheetFileName, code)), text);
        }

        private void LoadLangSheet(string sheetText, string code)
        {
            var sheetLines = sheetText.Split('\n'); // gdocs provides \n
            for (int i = 1; i < sheetLines.Length; i++)
            {
                // The formatting is odd here because gdocs adds quotes around columns and doubles each single quote
                var elements = sheetLines[i].Split(new[] { "\"," }, StringSplitOptions.None);
                var id = uint.Parse(elements[0].Substring(1));
                var name = elements[1].Substring(1);
                var locations = elements[4].Substring(1);
                var addtlInfo = elements[5].Substring(1, elements[5].Length - 2).Replace("\"\"", "\"");

                if (string.IsNullOrEmpty(name) || name == "Null BGM" || name == "test")
                    continue;

                if (!BgmPaths.TryGetValue(id, out var path))
                    continue;

                var bgm = new BgmInfo
                {
                    Title = name,
                    FilePath = path,
                    Location = locations,
                    AdditionalInfo = addtlInfo,
                    RowId = id,
                    Available = path.IsNullOrEmpty() || Services.DataManager.FileExists(path)
                };
                Bgms[id] = bgm;
            }

            SaveLocalSheet(sheetText, code);
        }

        private unsafe void Tick(IFramework framework)
        {
            var bgms = (BgmScene*)BgmSceneList.ToPointer();

            for (int sceneIdx = 0; sceneIdx < SceneCount; sceneIdx++)
            {
                if (bgms[sceneIdx].BgmReference == 0) continue;

                if (bgms[sceneIdx].BgmId != 0 && bgms[sceneIdx].BgmId != 9999)
                {
                    if (CurrentSongId != bgms[sceneIdx].BgmId)
                    {
                        SongChanged(bgms[sceneIdx].BgmId);
                    }

                    break;
                }
            }
        }

        private void SongChanged(uint songId)
        {
            Services.Log.Debug($"SongChanged {songId}");
            CurrentSongId = songId;
            OnBgmChange?.Invoke(CurrentSongId);
        }

        public override void Dispose()
        {
            base.Dispose();
            Services.Framework.Update -= Tick;
            disposeCts.Cancel();
            disposeCts.Dispose();
        }
    }
}