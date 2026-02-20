using System;

namespace TitleEdit.Data.Persistence
{
    public struct PresetModel
    {
        public const int CURRENT_VERSION = 6;

        public int? Version = CURRENT_VERSION;
        public string Name = "";
        public string Author = "";
        public CameraFollowMode CameraFollowMode = CameraFollowMode.Inherit;
        [Obsolete]
        public bool LastLocationMount = false;
        public LocationModel LocationModel = new();
        [NonSerialized]
        public string FileName = "";
        [NonSerialized]
        public string? Tooltip = null;
        [NonSerialized]
        public bool BuiltIn = false;
        [NonSerialized]
        public bool Vanilla = false;

        public PresetModel() { }
    }
}
