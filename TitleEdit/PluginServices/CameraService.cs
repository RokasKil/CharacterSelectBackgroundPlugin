using FFXIVClientStructs.FFXIV.Client.Game.Control;
using TitleEdit.Data.Camera;

namespace TitleEdit.PluginServices
{
    public class CameraService : AbstractService
    {
        public unsafe CameraManager* CameraManager => FFXIVClientStructs.FFXIV.Client.Game.Control.CameraManager.Instance();
        public unsafe LobbyCameraExpanded* LobbyCamera => (LobbyCameraExpanded*)CameraManager->LobbyCamera;
        public unsafe CameraExpanded* CurrentCamera => (CameraExpanded*)CameraManager->GetActiveCamera();

        public CameraService()
        {
        }
    }
}