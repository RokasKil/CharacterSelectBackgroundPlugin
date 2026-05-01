using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.Reflection;
using System.Threading.Tasks;
using TitleEdit.Utility;
using TitleEdit.Windows;

namespace TitleEdit;

public sealed class Plugin : IDalamudPlugin
{
    private const string CommandName = "/titleedit";
    private const string CommandNameAlias = "/te";

    public readonly WindowSystem WindowSystem = new("TitleEdit");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    private ConfigButtonOverlay ConfigButtonOverlay { get; init; }

    private bool canChangeUiVisibility = true;

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        Services.ConstructServices(pluginInterface, this);
        Services.LoadServiceData();
        if (!Services.ConfigurationService.SettingsMigrated)
        {
            Services.MigrationService.MigrateTitleScreenV2Presets();
            Services.MigrationService.MigrateTitleScreenV2Configuration();
        }

        Services.Framework.RunOnFrameworkThread(Services.InitServices).GetAwaiter().GetResult();

        // Load menu_icon.png from dll resources
        Services.PluginInterface.UiBuilder.RunWhenUiPrepared(() =>
        {
            var image = Services.TextureProvider.GetFromManifestResource(Assembly.GetExecutingAssembly(), "TitleEdit.menu_icon.png");
            Services.TitleScreenMenu.AddEntry("Title Edit Menu", image, ToggleConfigUI);
            return true;
        });

        ConfigWindow = new();
        MainWindow = new();
        ConfigButtonOverlay = new();

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        Services.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Title Edit configuration"
        });
        Services.CommandManager.AddHandler(CommandNameAlias, new CommandInfo(OnCommand)
        {
            ShowInHelp = false
        });

        Services.PluginInterface.UiBuilder.Draw += DrawUI;

        Services.PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
#if DEBUG
        Services.PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
#endif
        Services.Framework.Update += CheckHotkey;
    }

    public void Dispose()
    {
        Services.Framework.Update -= CheckHotkey;

        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();

        Services.CommandManager.RemoveHandler(CommandName);
        Services.CommandManager.RemoveHandler(CommandNameAlias);
        Services.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        if (args == "migrate presets")
        {
            var migratedCount = Services.MigrationService.MigrateTitleScreenV2Presets();
            Services.ChatGui.Print($"Migrated {migratedCount} presets", "Title Edit");
        }
        else if (args == "migrate settings")
        {
            if (Services.MigrationService.MigrateTitleScreenV2Configuration())
            {
                Services.ChatGui.Print($"Migrated v2 config", "Title Edit");
            }
            else
            {
                Services.ChatGui.Print($"Couldn't find v2 config", "Title Edit");
            }
        }
        else
        {
            ToggleConfigUI();
        }
    }

    private void DrawUI()
    {
        ConfigButtonOverlay.Draw();
        WindowSystem.Draw();
    }

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();

    private void CheckHotkey(IFramework framework)
    {
        // ctrl+t only on title screen (maybe? (not really but close enough))
        if (Services.KeyState[VirtualKey.CONTROL] &&
            Services.KeyState[VirtualKey.T] &&
            Services.ObjectTable.LocalPlayer == null
            && canChangeUiVisibility)
        {
            ToggleConfigUI();

#if DEBUG
            MainWindow.IsOpen = ConfigWindow.IsOpen;
#endif
            canChangeUiVisibility = false;
            Task.Delay(200).ContinueWith(_ => canChangeUiVisibility = true);
        }
    }
}