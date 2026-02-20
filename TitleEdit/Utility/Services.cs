using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TitleEdit.PluginServices;
using TitleEdit.PluginServices.Lobby;
using TitleEdit.PluginServices.Migration;
using TitleEdit.PluginServices.Preset;

namespace TitleEdit.Utility
{
    public class Services
    {
        [PluginService]
        public static IClientState ClientState { get; set; } = null!;
        [PluginService]
        public static ISigScanner SigScanner { get; set; } = null!;
        [PluginService]
        public static IDataManager DataManager { get; set; } = null!;
        [PluginService]
        public static ICondition Condition { get; set; } = null!;
        [PluginService]
        public static IGameGui GameGui { get; set; } = null!;
        [PluginService]
        public static IPluginLog Log { get; set; } = null!;
        [PluginService]
        public static IGameInteropProvider GameInteropProvider { get; set; } = null!;
        [PluginService]
        public static IFramework Framework { get; set; } = null!;
        [PluginService]
        public static ICommandManager CommandManager { get; set; } = null!;
        [PluginService]
        public static IAddonLifecycle AddonLifecycle { get; set; } = null!;
        [PluginService]
        public static ITextureProvider TextureProvider { get; set; } = null!;
        [PluginService]
        public static ITitleScreenMenu TitleScreenMenu { get; set; } = null!;
        [PluginService]
        public static IKeyState KeyState { get; set; } = null!;
        [PluginService]
        public static INotificationManager NotificationManager { get; set; } = null!;
        [PluginService]
        public static IChatGui ChatGui { get; set; } = null!;
        [PluginService]
        public static IObjectTable ObjectTable { get; set; } = null!;
        [PluginService]
        public static IPlayerState PlayerState { get; set; } = null!;
        [PluginService]
        [Experimental("DAL_RPC")]
        public static IPluginLinkHandler PluginLinkHandler { get; set; } = null!;
        public static IDalamudPluginInterface PluginInterface { get; set; } = null!;
        public static ConfigurationService ConfigurationService { get; set; } = null!;
        public static LayoutService LayoutService { get; set; } = null!;
        public static LobbyService LobbyService { get; set; } = null!;
        public static LocationService LocationService { get; set; } = null!;
        public static BgmService BgmService { get; set; } = null!;
        public static WeatherService WeatherService { get; set; } = null!;
        public static PresetService PresetService { get; set; } = null!;
        public static CharactersService CharactersService { get; set; } = null!;
        public static MountService MountService { get; set; } = null!;
        public static BoneService BoneService { get; set; } = null!;
        public static MigrationService MigrationService { get; set; } = null!;
        public static CameraService CameraService { get; set; } = null!;
        public static GroupService GroupService { get; set; } = null!;
        public static ExpansionService ExpansionService { get; set; } = null!;
        public static HousingService HousingService { get; set; } = null!;
        public static UriService UriService { get; set; } = null!;
        public static ShareService ShareService { get; set; } = null!;
        public static Plugin Plugin { get; set; } = null!;

        private static List<AbstractService> ServiceList = [];

        private static bool Disposed = false;

        public static void ConstructServices(IDalamudPluginInterface pluginInterface, Plugin plugin)
        {
            pluginInterface.Create<Services>();
            Plugin = plugin;
            PluginInterface = pluginInterface;
            try
            {
                ServiceList.Add(LayoutService = new());
                ServiceList.Add(LobbyService = new());
                ServiceList.Add(LocationService = new());
                ServiceList.Add(BgmService = new());
                ServiceList.Add(WeatherService = new());
                ServiceList.Add(PresetService = new());
                ServiceList.Add(CharactersService = new());
                ServiceList.Add(MountService = new());
                ServiceList.Add(BoneService = new());
                ServiceList.Add(MigrationService = new());
                ServiceList.Add(CameraService = new());
                ServiceList.Add(GroupService = new());
                ServiceList.Add(ExpansionService = new());
                ServiceList.Add(HousingService = new());
                ServiceList.Add(UriService = new());
                ServiceList.Add(ShareService = new());
            }
            catch
            {
                Dispose();
                throw;
            }

            ConfigurationService = ConfigurationService.Initialize(pluginInterface);
        }

        public static void LoadServiceData()
        {
            try
            {
                ServiceList.ForEach(service => service.LoadData());
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public static void InitServices()
        {
            try
            {
                ServiceList.ForEach(service => service.Init());
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public static void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            ServiceList.ForEach(service => service.Dispose());
        }
    }
}
