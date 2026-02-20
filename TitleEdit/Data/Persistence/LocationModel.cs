using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using TitleEdit.Data.Character;
using TitleEdit.Data.Lobby;
using TitleEdit.Utility;

namespace TitleEdit.Data.Persistence
{
    // Could seperate these into title screen and character select but I kinda want to allow users to easily convert between the two maybe (also am lazy)
    public struct LocationModel
    {
        public const int CURRENT_VERSION = 6;
        // Festival count post 7.4
        public const int FESTIVAL_COUNT = 8;
        // Festival count pre 7.4
        public const int OLD_FESTIVAL_COUNT = 4;

        public int Version = CURRENT_VERSION;
        public LocationType LocationType = LocationType.TitleScreen;
        public TitleScreenLogo TitleScreenLogo = TitleScreenLogo.Dawntrail;
        public string TerritoryPath = ""; // TODO: Why am I even saving both
        public ushort TerritoryTypeId;
        public uint LayoutTerritoryTypeId;
        public uint LayoutLayerFilterKey;
        public Vector3 Position;
        public Vector3 CameraPosition;
        public float Rotation;
        public float Yaw;   // side
        public float Roll;  // roll
        public float Pitch; // up/down
        public float Fov = 1;
        public byte WeatherId;
        public ushort TimeOffset;
        public uint BgmId = 0; // TODO: Why am I even saving both
        public string? BgmPath;
        public MovementMode MovementMode = MovementMode.Normal;
        public MountModel Mount;
        public HashSet<ulong> Active = [];
        public HashSet<ulong> Inactive = [];
        public Dictionary<ulong, short> VfxTriggerIndexes = [];
        // Used to be an uint[], the converter converts those to Festival struct that is made out of 2 ushort
        [JsonConverter(typeof(FestivalJsonConverter))]
        public Festival[] Festivals = new Festival[FESTIVAL_COUNT];
        public bool SaveLayout = false;
        public bool UseVfx = true;
        public bool SaveHousing = true;
        public bool SaveFestivals = true;
        // Used to force the title screen expansion to specific one and let the game handle the scene loading natively
        public TitleScreenExpansion? TitleScreenOverride = null;
        public UiColorModel UiColor = UiColors.Dawntrail;
        public TitleScreenMovie TitleScreenMovie = TitleScreenMovie.Unspecified;
        public bool UseLiveTime = false;

        public List<HousingFurnitureModel>? Furniture = null;
        public List<HousingPlotModel>? Plots = null;
        public HousingEstateModel? Estate = null;

        // Only set when used with a preset
        [NonSerialized]
        public CameraFollowMode CameraFollowMode = CameraFollowMode.Inherit;

        // What to say in the toast notification
        [NonSerialized]
        public string ToastNotificationText = "";

        // Currently only used for seasonal easter eggs
        [NonSerialized]
        public List<NpcModel>? Npcs = null;

        public LocationModel() { }
    }
}
