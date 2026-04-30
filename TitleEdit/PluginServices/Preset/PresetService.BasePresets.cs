using System;
using System.Numerics;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Preset
{
    public partial class PresetService
    {
        private void LoadBasePresets()
        {
            // Title screens
            AddVanillaTitleScreenPresets();
            AddTitleScreenPresets();

            //Character select
            AddVanillaCharacterSelectPresets();
            AddCharacterSelectPresets();
        }

        private void AddVanillaTitleScreenPresets()
        {
            AddPreset(new()
            {
                Name = "A Realm Reborn",
                Tooltip = "Vanilla A Realm Reborn title screen",
                FileName = "?/ARealmReborn.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.ARealmReborn,
                    ToastNotificationText = "Now displaying: A Realm Reborn",
                    TitleScreenLogo = TitleScreenLogo.ARealmReborn,
                    UiColor = UiColors.ARealmReborn,
                    TitleScreenMovie = TitleScreenMovie.ARealmReborn,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-9.00f, 10.90f, 25.90f),
                            Rotation = -2.18f,
                            Scale = 0.60f
                        },

                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-1.20f, -1.10f, 4.20f),
                            Rotation = 2.55f,
                            Scale = 3.50f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "Heavensward",
                Tooltip = "Vanilla Heavensward title screen",
                FileName = "?/Heavensward.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.Heavensward,
                    TitleScreenLogo = TitleScreenLogo.Heavensward,
                    UiColor = UiColors.Heavensward,
                    TitleScreenMovie = TitleScreenMovie.Heavensward,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-0.26f, -0.10f, 4.90f),
                            Rotation = -0.49f,
                            Scale = 6.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "Stormblood",
                Tooltip = "Vanilla Stormblood title screen",
                FileName = "?/Stormblood.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.Stormblood,
                    TitleScreenLogo = TitleScreenLogo.Stormblood,
                    UiColor = UiColors.Stormblood,
                    TitleScreenMovie = TitleScreenMovie.Stormblood,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-0.90f, 2.60f, 5.30f),
                            Rotation = -0.59f,
                            Scale = 0.70f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-0.70f, 3.02f, 5.86f),
                            Rotation = 0.17f,
                            Scale = 1.80f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "Shadowbringers",
                Tooltip = "Vanilla Shadowbringers title screen",
                FileName = "?/Shadowbringers.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.Shadowbringers,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    UiColor = UiColors.Shadowbringers,
                    TitleScreenMovie = TitleScreenMovie.Shadowbringers,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(0.50f, -0.90f, 4.00f),
                            Rotation = -1.12f,
                            Scale = 2.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "Endwalker",
                Tooltip = "Vanilla Endwalker title screen",
                FileName = "?/Endwalker.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.Endwalker,
                    TitleScreenLogo = TitleScreenLogo.Endwalker,
                    UiColor = UiColors.Endwalker,
                    TitleScreenMovie = TitleScreenMovie.Endwalker,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-26.90f, 4.00f, -196.60f),
                            Rotation = 1.31f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-25.60f, 4.20f, -198.90f),
                            Rotation = 0.84f,
                            Scale = 1.60f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "Dawntrail",
                Tooltip = "Vanilla Dawntrail title screen",
                FileName = "?/Dawntrail.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenOverride = TitleScreenExpansion.Dawntrail,
                    TitleScreenLogo = TitleScreenLogo.Dawntrail,
                    UiColor = UiColors.Dawntrail,
                    TitleScreenMovie = TitleScreenMovie.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-1.50f, -1.20f, 161.30f),
                            Rotation = 1.31f,
                            Scale = 39.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(203.90f, 4.50f, 351.70f),
                            Rotation = 0.14f,
                            Scale = 74.40f
                        }
                    ]
                }
            });
        }

        private void AddTitleScreenPresets()
        {
            AddPreset(new()
            {
                Name = "TE_Amaurot 1",
                FileName = "?/TE_Amaurot 1.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    TerritoryPath = "ex3/01_nvt_n4/fld/n4f6/level/n4f6",
                    TerritoryTypeId = 818,
                    CameraPosition = new(22.773487f, -61.84117f, 757.2081f),
                    Yaw = -1.1805015f,
                    Roll = 0.0f,
                    Pitch = 0.29725394f,
                    Fov = 45.0f,
                    WeatherId = 1,
                    TimeOffset = 0,
                    BgmId = 0,
                    BgmPath = "music/ffxiv/orchestrion/bgm_orch_430.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-316.60f, -72.60f, 829.80f),
                            Rotation = 1.29f,
                            Scale = 6.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-492.80f, 19.70f, 1203.70f),
                            Rotation = 2.90f,
                            Scale = 27.30f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Amaurot 2",
                FileName = "?/TE_Amaurot 2.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    TerritoryPath = "ex3/01_nvt_n4/dun/n4d6/level/n4d6",
                    TerritoryTypeId = 838,
                    CameraPosition = new(-0.38227558f, -491.87146f, 686.6074f),
                    Yaw = 3.1415927f,
                    Roll = 0.0f,
                    Pitch = -0.42794815f,
                    Fov = 45.0f,
                    WeatherId = 120,
                    TimeOffset = 0,
                    BgmId = 692,
                    BgmPath = "music/ex3/BGM_EX3_Dan_D06.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(0.00f, -719.10f, -49.40f),
                            Rotation = -0.31f,
                            Scale = 13.80f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(0.00f, -671.60f, -1978.00f),
                            Rotation = 0.09f,
                            Scale = 467.60f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Amaurot 3",
                FileName = "?/TE_Amaurot 3.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ex3/01_nvt_n4/dun/n4d6/level/n4d6",
                    TerritoryTypeId = 838,
                    CameraPosition = new(-174.61586f, -704.9572f, -55.22666f),
                    Yaw = -0.84668314f,
                    Roll = 0.0f,
                    Pitch = 0.40988576f,
                    Fov = 45.0f,
                    WeatherId = 2,
                    TimeOffset = 0,
                    BgmId = 647,
                    BgmPath = "music/ex3/BGM_EX3_Event_02.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-200.40f, -701.20f, -32.50f),
                            Rotation = 0.70f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-200.20f, -699.60f, -32.50f),
                            Rotation = 0.70f,
                            Scale = 1.50f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TEE_AzysLla",
                FileName = "?/TEE_AzysLla.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ex1/03_abr_a2/fld/a2f2/level/a2f2",
                    TerritoryTypeId = 402,
                    CameraPosition = new(-582.60913f, 50.544624f, -251.91136f),
                    Yaw = 0.9263441f,
                    Roll = 0.0f,
                    Pitch = 0.49396738f,
                    Fov = 1.0f,
                    WeatherId = 54,
                    TimeOffset = 0,
                    BgmId = 385,
                    BgmPath = "music/ex1/BGM_EX1_Boss04.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-496.90f, 417.10f, 207.60f),
                            Rotation = -1.75f,
                            Scale = 7.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-408.50f, 417.10f, 95.50f),
                            Rotation = -1.76f,
                            Scale = 27.30f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Carrotorium",
                FileName = "?/TE_Carrotorium.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Endwalker,
                    TerritoryPath = "ex4/04_uvs_u5/fld/u5f1/level/u5f1",
                    TerritoryTypeId = 959,
                    CameraPosition = new(-510.2475f, -150.07487f, -522.2673f),
                    Yaw = -1.8643512f,
                    Roll = 0.0f,
                    Pitch = -0.8711339f,
                    Fov = 1.0f,
                    WeatherId = 2,
                    TimeOffset = 0,
                    BgmId = 880,
                    BgmPath = "music/ex4/BGM_EX4_Dan_D07.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-521.00f, -158.10f, -523.30f),
                            Rotation = 1.52f,
                            Scale = 0.60f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-518.60f, -158.10f, -523.40f),
                            Rotation = -1.76f,
                            Scale = 1.70f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Central Shroud",
                FileName = "?/TE_Central Shroud.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.ARealmReborn,
                    TerritoryPath = "ffxiv/fst_f1/fld/f1f1/level/f1f1",
                    TerritoryTypeId = 148,
                    CameraPosition = new(189.35226f, -29.25525f, 375.48474f),
                    Yaw = 1.5353248f,
                    Roll = 0.0f,
                    Pitch = 0.081796646f,
                    Fov = 45.0f,
                    WeatherId = 1,
                    TimeOffset = 640,
                    BgmId = 0,
                    BgmPath = "music/ffxiv/orchestrion/bgm_orch_391.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(201.70f, -31.90f, 382.00f),
                            Rotation = -2.71f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(197.00f, -32.10f, 374.10f),
                            Rotation = -1.10f,
                            Scale = 1.50f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Dalamud",
                FileName = "?/TE_Dalamud.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.ARealmReborn,
                    TerritoryPath = "ex3/01_nvt_n4/fld/n4fe/level/n4fe",
                    TerritoryTypeId = 897,
                    CameraPosition = new(192.5f, 23.0f, 128.5f),
                    Yaw = 2.5567405f,
                    Roll = 0.0f,
                    Pitch = 0.3920805f,
                    Fov = 45.0f,
                    WeatherId = 101,
                    TimeOffset = 0,
                    BgmId = 198,
                    BgmPath = "music/ffxiv/BGM_Con_Neal.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(269.60f, 69.20f, -132.20f),
                            Rotation = -2.74f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(234.90f, 31.90f, -4.10f),
                            Rotation = -2.79f,
                            Scale = 5.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Dalamud 2",
                FileName = "?/TE_Dalamud 2.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ffxiv/wil_w1/twn/w1t1/level/w1t1",
                    TerritoryTypeId = 130,
                    CameraPosition = new(-124.29569f, 20.383795f, -96.238174f),
                    Yaw = 1.9675864f,
                    Roll = 0.0f,
                    Pitch = 0.08992102f,
                    Fov = 45.0f,
                    WeatherId = 101,
                    TimeOffset = 800,
                    BgmId = 237,
                    BgmPath = "music/ffxiv/BGM_Con_Bahamut_Bigboss2.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-84.60f, 4.00f, -125.40f),
                            Rotation = 1.85f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-84.60f, 4.00f, -124.40f),
                            Rotation = 1.90f,
                            Scale = 2.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Don't Ask",
                FileName = "?/TE_Don't Ask.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ffxiv/wil_w1/twn/w1tz/level/w1tz",
                    TerritoryTypeId = 796,
                    CameraPosition = new(70.45863f, 12.015016f, 149.7881f),
                    Yaw = -0.9926575f,
                    Roll = 0.0f,
                    Pitch = 0.12776367f,
                    Fov = 45.0f,
                    WeatherId = 2,
                    TimeOffset = 1100,
                    BgmId = 538,
                    BgmPath = "music/ffxiv/BGM_Minigame_Egghunt.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(56.20f, 16.10f, 163.20f),
                            Rotation = 2.57f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(66.00f, 11.00f, 152.70f),
                            Rotation = 2.60f,
                            Scale = 1.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Garlemald",
                FileName = "?/TE_Garlemald.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    TerritoryPath = "ex3/01_nvt_n4/evt/n4ed/level/n4ed",
                    TerritoryTypeId = 0,
                    CameraPosition = new(-472.1514f, 280.8405f, -953.2695f),
                    Yaw = 1.0745571f,
                    Roll = 0.0f,
                    Pitch = 0.4948496f,
                    Fov = 1.0f,
                    WeatherId = 2,
                    TimeOffset = 2105,
                    BgmId = 761,
                    BgmPath = "music/ex3/BGM_EX3_Event_23.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-206.30f, 542.70f, -814.50f),
                            Rotation = -0.51f,
                            Scale = 6.90f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-206.30f, 553.90f, -813.30f),
                            Rotation = -0.51f,
                            Scale = 9.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Hell's Kier",
                FileName = "?/TE_Hell's Kier.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Stormblood,
                    TerritoryPath = "ex2/02_est_e3/fld/e3fe/level/e3fe",
                    TerritoryTypeId = 810,
                    CameraPosition = new(184.04657f, -39.467655f, 54.550262f),
                    Yaw = -1.3519993f,
                    Roll = 0.0f,
                    Pitch = 0.1990427f,
                    Fov = 1.24f,
                    WeatherId = 104,
                    TimeOffset = 0,
                    BgmId = 582,
                    BgmPath = "music/ex2/BGM_EX2_Ban_25.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(142.50f, -8.90f, 56.90f),
                            Rotation = 2.50f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(146.50f, -8.90f, 59.90f),
                            Rotation = 1.43f,
                            Scale = 2.70f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Ishgard",
                FileName = "?/TE_Ishgard.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Heavensward,
                    TerritoryPath = "ex1/01_roc_r2/twn/r2t1/level/r2t1",
                    TerritoryTypeId = 418,
                    CameraPosition = new(-251.92082f, 8.874063f, 166.83122f),
                    Yaw = 2.3122256f,
                    Roll = 0.0f,
                    Pitch = 0.41671044f,
                    Fov = 45.0f,
                    WeatherId = 15,
                    TimeOffset = 1985,
                    BgmId = 318,
                    BgmPath = "music/ex1/BGM_EX1_Event_Start.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-131.60f, 5.20f, 41.40f),
                            Rotation = -0.24f,
                            Scale = 1.10f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-16.40f, 1.80f, 97.60f),
                            Rotation = -1.66f,
                            Scale = 5.50f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Kozama'uka - Midday",
                FileName = "?/TE_Kozama'uka - Midday.json",
                Author = "bevral",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Dawntrail,
                    TerritoryPath = "ex5/02_ykt_y6/fld/y6f2/level/y6f2",
                    TerritoryTypeId = 1188,
                    CameraPosition = new(-508.61078f, 65.44885f, -416.0996f),
                    Yaw = 1.1428189f,
                    Roll = 0.0f,
                    Pitch = 0.09602679f,
                    Fov = 1.08f,
                    WeatherId = 2,
                    TimeOffset = 1100,
                    BgmId = 20063,
                    BgmPath = "music/ex5/BGM_EX5_Field_Safe02.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-321.30f, 0.60f, -430.80f),
                            Rotation = -1.17f,
                            Scale = 1.70f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-372.90f, -0.30f, -337.30f),
                            Rotation = -0.35f,
                            Scale = 3.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Limsa Lominsa",
                FileName = "?/TE_Limsa Lominsa.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.ARealmReborn,
                    TerritoryPath = "ffxiv/sea_s1/twn/s1t1/level/s1t1",
                    TerritoryTypeId = 128,
                    CameraPosition = new(25.326021f, 48.90745f, 152.9382f),
                    Yaw = 0.36621737f,
                    Roll = 0.0f,
                    Pitch = 0.047502767f,
                    Fov = 1.0f,
                    WeatherId = 1,
                    TimeOffset = 400,
                    BgmId = 747,
                    BgmPath = "music/ex1/BGM_EX1_Hukko_Finish01.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(25.60f, 44.50f, 210.50f),
                            Rotation = 0.26f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(26.60f, 44.50f, 212.00f),
                            Rotation = -2.78f,
                            Scale = 2.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Reisen Temple",
                FileName = "?/TE_Reisen Temple.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ex2/02_est_e3/evt/e3e8/level/e3e8",
                    TerritoryTypeId = 764,
                    CameraPosition = new(-10.635f, -54.55f, -165.71f),
                    Yaw = 2.8003855f,
                    Roll = 0.0f,
                    Pitch = 0.008002617f,
                    Fov = 45.0f,
                    WeatherId = 2,
                    TimeOffset = 0,
                    BgmId = 519,
                    BgmPath = "music/ex2/BGM_EX2_Field_Iroha.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(85.60f, -96.70f, -288.80f),
                            Rotation = -2.02f,
                            Scale = 1.40f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(63.40f, -87.00f, -532.50f),
                            Rotation = 0.24f,
                            Scale = 19.70f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Shadowbringers 2",
                FileName = "?/TE_Shadowbringers 2.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    TerritoryPath = "ex3/01_nvt_n4/twn/n4t1/level/n4t1",
                    TerritoryTypeId = 819,
                    CameraPosition = new(-508.77274f, 0.96345747f, 74.34585f),
                    Yaw = 1.6631284f,
                    Roll = 0.0f,
                    Pitch = 0.3642672f,
                    Fov = 45.0f,
                    WeatherId = 118,
                    TimeOffset = 0,
                    BgmId = 705,
                    BgmPath = "music/ex3/BGM_EX3_EndCredit01.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-121.30f, 0.00f, 267.10f),
                            Rotation = -0.47f,
                            Scale = 2.20f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-121.30f, 0.00f, 321.80f),
                            Rotation = 2.46f,
                            Scale = 10.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Titania's Forest",
                FileName = "?/TE_Titania's Forest.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Shadowbringers,
                    TerritoryPath = "ex3/01_nvt_n4/fld/n4fa/level/n4fa",
                    TerritoryTypeId = 354,
                    CameraPosition = new(68.31682f, 7.3394575f, 136.57358f),
                    Yaw = -2.013775f,
                    Roll = 0.0f,
                    Pitch = 0.22415328f,
                    Fov = 45.0f,
                    WeatherId = 123,
                    TimeOffset = 0,
                    BgmId = 700,
                    BgmPath = "music/ffxiv/BGM_Ride_Titania.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-636.20f, -281.40f, -211.50f),
                            Rotation = 0.19f,
                            Scale = 75.50f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(-519.50f, 8.50f, -18.20f),
                            Rotation = 2.08f,
                            Scale = 36.30f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Tuliyollal Night",
                FileName = "?/TE_Tuliyollal Night.json",
                Author = "alyssile",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.Dawntrail,
                    TerritoryPath = "ex5/02_ykt_y6/twn/y6t1/level/y6t1",
                    TerritoryTypeId = 1185,
                    CameraPosition = new(130.34209f, -16.301905f, 172.8012f),
                    Yaw = -2.5165493f,
                    Roll = 0.0f,
                    Pitch = 0.14567117f,
                    Fov = 1.0f,
                    WeatherId = 1,
                    TimeOffset = 2200,
                    BgmId = 20046,
                    BgmPath = "music/ex5/BGM_EX5_Town_T_Night.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(132.90f, -16.70f, 146.70f),
                            Rotation = -0.42f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(71.50f, -18.10f, 132.90f),
                            Rotation = 1.26f,
                            Scale = 3.10f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Werlyt",
                FileName = "?/TE_Werlyt.json",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.TitleScreen,
                    TitleScreenLogo = TitleScreenLogo.None,
                    TerritoryPath = "ex3/01_nvt_n4/fld/n4fg/level/n4fg",
                    TerritoryTypeId = 934,
                    CameraPosition = new(78.44922f, 17.70825f, 101.83251f),
                    Yaw = -2.9742289f,
                    Roll = 0.0f,
                    Pitch = -0.006300778f,
                    Fov = 45.0f,
                    WeatherId = 105,
                    TimeOffset = 0,
                    BgmId = 767,
                    BgmPath = "music/ex3/BGM_EX3_Field_Welrit.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    UiColor = UiColors.Dawntrail,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(47.50f, 7.90f, 42.50f),
                            Rotation = -0.79f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(42.30f, 12.10f, 59.70f),
                            Rotation = 1.26f,
                            Scale = 2.00f
                        }
                    ]
                }
            });
        }

        private void AddVanillaCharacterSelectPresets()
        {
            // Character select
            AddPreset(new()
            {
                Name = "Aetherial Sea",
                Tooltip = "Vanilla Aetherial Sea character select screen",
                FileName = "?/AetherialSea.json",
                Vanilla = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ffxiv/zon_z1/chr/z1c1/level/z1c1",
                    Position = Vector3.Zero,
                    Rotation = 0,
                    WeatherId = 2,
                    TimeOffset = 0,
                    BgmPath = "music/ffxiv/BGM_System_Chara.scd",
                    ToastNotificationText = "Now displaying: Aetherial Sea",
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-0.70f, 0.00f, -0.50f),
                            Rotation = 0.35f,
                            Scale = 0.15f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(0.70f, 0.00f, -0.20f),
                            Rotation = -0.40f,
                            Scale = 1.00f
                        }
                    ]
                }
            });
        }

        private void AddCharacterSelectPresets()
        {
            AddPreset(new()
            {
                Name = "TE_Candy Store",
                FileName = "?/TE_Candy Store.json",
                Author = "Thorian",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex3/01_nvt_n4/evt/n4e9/level/n4e9",
                    TerritoryTypeId = 892,
                    LayoutTerritoryTypeId = 892,
                    LayoutLayerFilterKey = 0,
                    Position = new(77.35472f, 0.15887594f, 50.74012f),
                    Rotation = -0.7853982f,
                    WeatherId = 2,
                    TimeOffset = 1228,
                    BgmId = 719,
                    BgmPath = "music/ex3/BGM_EX3_BanFort_Fairy_Good.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(82.70f, 0.73f, 45.00f),
                            Rotation = 0.00f,
                            Scale = 0.80f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(84.90f, 1.23f, 45.50f),
                            Rotation = -1.12f,
                            Scale = 1.40f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Convocation",
                FileName = "?/TE_Convocation.json",
                Author = "bot_",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ffxiv/zon_z1/evt/z1e7/level/z1e7",
                    TerritoryTypeId = 0,
                    LayoutTerritoryTypeId = 0,
                    LayoutLayerFilterKey = 0,
                    Position = new(0.0f, 0.0f, -0.3f),
                    Rotation = 0.0f,
                    WeatherId = 2,
                    TimeOffset = 1200,
                    BgmId = 58,
                    BgmPath = "music/ffxiv/BGM_Field_Danger2.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(10.20f, 1.00f, -0.10f),
                            Rotation = -1.57f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-10.20f, 1.00f, -0.10f),
                            Rotation = 1.57f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-9.40f, 1.00f, 3.20f),
                            Rotation = 1.83f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-8.00f, 1.00f, 5.90f),
                            Rotation = 2.11f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-5.80f, 1.00f, 8.20f),
                            Rotation = 2.46f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-9.40f, 1.00f, -3.20f),
                            Rotation = 1.31f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-8.00f, 1.00f, -5.90f),
                            Rotation = 0.89f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(-5.80f, 1.00f, -8.20f),
                            Rotation = 0.68f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(9.40f, 1.00f, 3.20f),
                            Rotation = -1.83f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(8.00f, 1.00f, 5.90f),
                            Rotation = -2.11f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(5.80f, 1.00f, 8.20f),
                            Rotation = -2.46f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(9.40f, 1.00f, -3.20f),
                            Rotation = -1.31f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(8.00f, 1.00f, -5.90f),
                            Rotation = -0.89f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(5.80f, 1.00f, -8.20f),
                            Rotation = -0.68f,
                            Scale = 1.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Elysion",
                FileName = "?/TE_Elysion.json",
                Author = "Speedas",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex4/04_uvs_u5/evt/u5e3/level/u5e3",
                    TerritoryTypeId = 1073,
                    LayoutTerritoryTypeId = 0,
                    LayoutLayerFilterKey = 268557,
                    Position = new(91.0f, 497.26218f, -8.9f),
                    Rotation = -1.5707964f,
                    WeatherId = 2,
                    TimeOffset = 2205,
                    BgmId = 929,
                    BgmPath = "music/ex4/BGM_EX4_BanFort_Omi_Good.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(173.40f, 492.96f, -2.80f),
                            Rotation = -2.25f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(77.10f, 496.06f, -17.30f),
                            Rotation = 2.37f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(77.40f, 496.06f, -19.70f),
                            Rotation = 0.35f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(79.20f, 496.06f, -18.15f),
                            Rotation = -1.52f,
                            Scale = 1.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Jungle",
                FileName = "?/TE_Jungle.json",
                Author = "Thorian",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ffxiv/sea_s1/dun/s1d5/level/s1d5",
                    TerritoryTypeId = 361,
                    LayoutTerritoryTypeId = 0,
                    LayoutLayerFilterKey = 0,
                    Position = new(263.6f, 63.237f, 160.25f),
                    Rotation = 3.1415927f,
                    WeatherId = 2,
                    TimeOffset = 1126,
                    BgmId = 742,
                    BgmPath = "music/ex3/BGM_EX3_Event_20.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(250.80f, 60.64f, 129.65f),
                            Rotation = 0.31f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(237.00f, 66.44f, 175.45f),
                            Rotation = 0.61f,
                            Scale = 2.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Nightbloom",
                FileName = "?/TE_Nightbloom.json",
                Author = "Kamgigari",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex2/02_est_e3/fld/e3fd/level/e3fd",
                    TerritoryTypeId = 778,
                    LayoutTerritoryTypeId = 0,
                    LayoutLayerFilterKey = 0,
                    Position = new(100.0f, 0.0f, 100.0f),
                    Rotation = 0.0f,
                    WeatherId = 99,
                    TimeOffset = 0,
                    BgmId = 542,
                    BgmPath = "music/ex2/BGM_EX2_Ban_17.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(102.70f, 6.30f, 143.20f),
                            Rotation = 3.14f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(104.40f, 6.30f, 141.50f),
                            Rotation = -2.84f,
                            Scale = 2.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_The Dancing Plague",
                FileName = "?/TE_The Dancing Plague.json",
                Author = "Speedas",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex3/01_nvt_n4/fld/n4fa/level/n4fa",
                    TerritoryTypeId = 845,
                    LayoutTerritoryTypeId = 845,
                    LayoutLayerFilterKey = 0,
                    Position = new(100.0f, 0.0f, 100.0f),
                    Rotation = 0.0f,
                    WeatherId = 122,
                    TimeOffset = 0,
                    BgmId = 683,
                    BgmPath = "music/ex3/BGM_EX3_Ban_01.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(100.00f, 12.50f, 38.20f),
                            Rotation = 0.00f,
                            Scale = 1.50f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(94.80f, 2.60f, 55.80f),
                            Rotation = 0.86f,
                            Scale = 3.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(105.20f, 2.60f, 55.80f),
                            Rotation = -0.86f,
                            Scale = 3.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Zero Domain",
                FileName = "?/TE_Zero Domain.json",
                Author = "bot_",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex4/05_zon_z5/evt/z5e4/level/z5e4",
                    TerritoryTypeId = 1077,
                    LayoutTerritoryTypeId = 0,
                    LayoutLayerFilterKey = 0,
                    Position = new(0.0f, -0.393f, -20.05f),
                    Rotation = 3.1415927f,
                    WeatherId = 171,
                    TimeOffset = 716,
                    BgmId = 928,
                    BgmPath = "music/ex4/BGM_EX4_Event_28.scd",
                    SaveLayout = false,
                    SaveFestivals = false,
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(0.70f, -0.39f, -87.25f),
                            Rotation = -2.84f,
                            Scale = 1.00f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(69.60f, 15.81f, -17.15f),
                            Rotation = -1.78f,
                            Scale = 3.00f
                        }
                    ]
                }
            });
            AddPreset(new()
            {
                Name = "TE_Futures Rewritten",
                FileName = "?/TE_Futures Rewritten.json",
                Author = "Speedas",
                BuiltIn = true,
                LocationModel = new()
                {
                    LocationType = LocationType.CharacterSelect,
                    TerritoryPath = "ex3/01_nvt_n4/goe/n4gw/level/n4gw",
                    TerritoryTypeId = 1238,
                    LayoutTerritoryTypeId = 1238,
                    LayoutLayerFilterKey = 0,
                    Position = new(100.0f, 0f, 100.0f),
                    Rotation = 0f,
                    WeatherId = 1,
                    TimeOffset = 0,
                    BgmId = 742,
                    BgmPath = "music/ex3/BGM_EX3_Event_20.scd",
                    SaveLayout = true,
                    UseVfx = true,
                    Active = { 10861454, 10875119, 10875146, 10875147, 10875179, 10875180, 10875256, 10875260, 10875261, 10875262, 10875263, 10875264, 10875265, 10875266, 10875267, 10875268, 10875269, 10875270, 10875271, 10875272, 10875273, 10875274, 10875275, 10875276, 10875277, 10875279, 10875281, 10875282, 10875784, 10875787, 10875788, 10875789, 10875790, 10875791, 10875792, 10875793, 10875794, 10875795, 10875796, 10875797, 10875798, 10875799, 10875800, 10875801, 10875802, 10875803, 10875804, 10875805, 10875806, 10875807, 10875808, 10875809, 10875810, 10875811, 10875812, 10875813, 10875814, 10875815, 10875816, 10875817, 10875818, 10875819, 10875820, 10875821, 10875822, 10875823, 10875824, 10875825, 10875826, 10875827, 10875828, 10875829, 10875830, 10875831, 10875832, 10875833, 10875834, 10875835, 10875836, 10875837, 10875838, 10875839, 10875840, 10875841, 10875842, 10875843, 10875844, 10875845, 10875846, 10875847, 10875848, 10875849, 10875850, 10875851, 10875852, 10875853, 10875854, 10875855, 10875856, 10875857, 10875858, 10875859, 10875860, 10875861, 10875862, 10875863, 10875864, 10875865, 10875866, 10875867, 10876127, 10876128, 10876129, 10876130, 10876131, 10876132, 10876133, 10876134, 10876135, 10876136, 10876137, 10876138, 10876139, 10876140, 10876141, 10876142, 10876143, 10876144, 10876145, 10876146, 10876147, 10876148, 10876149, 10876150, 10876151, 10876152, 10876153, 10876154, 10876155, 10876156, 10876157, 10876158, 10876159, 10876160, 10876161, 10876162, 10876163, 10876164, 10876165, 10876166, 10876167, 10876168, 10876169, 10876170, 10876171, 10876172, 10876173, 10876174, 10876175, 10876176, 10876177, 10876178, 10876179, 10876180, 10876181, 10876182, 10876183, 10876184, 10876185, 10876186, 10876187, 10876188, 10876189, 10876190, 10876191, 10876192, 10876193, 10876194, 10876195, 10876196, 10876197, 10876198, 10876199, 10876200, 10876201, 10876202, 10876203, 10876204, 10876205, 10876206, 10876207, 10876208, 10876209, 10876210, 10876211, 10876212, 10876213, 10876214, 10876215, 10876216, 10876217, 10876218, 10876219, 10876220, 10876221, 10876222, 10876223, 10876224, 10876225, 10876226, 10876227, 10876228, 10876229, 10876230, 10876231, 10876232, 10876233, 10876234, 10876235, 10876236, 10876237, 10876238, 10876239, 10876240, 10876241, 10876242, 10876243, 10876244, 10876245, 10876246, 10876247, 10876248, 10876249, 10876250, 10876251, 10876252, 10876253, 10876254, 10876255, 10876256, 10876257, 10876258, 10876259, 10876260, 10876261, 10876262, 10876263, 10876264, 10876265, 10876266, 10876267, 10876268, 10876269, 10876270, 10876271, 10876272, 10876273, 10876274, 10876275, 10876276, 10876277, 10876278, 10876279, 10876280, 10876281, 10876282, 10876283, 10876284, 10876285, 10876286, 10876287, 10876288, 10876289, 10876290, 10876291, 10876292, 10876293, 10876294, 10876295, 10876296, 10876297, 10876298, 10876299, 10876300, 10876301, 10876302, 10876303, 10876304, 10876305, 10876306, 10876307, 10876308, 10876309, 10876310, 10876311, 10876312, 10876313, 10876314, 10876315, 10876316, 10876317, 10876318, 10876319, 10876320, 10876321, 10876322, 10876323, 10876324, 10876325, 10876326, 10876327, 10876328, 10876329, 10876330, 10876331, 10876332, 10876333, 10876334, 10876335, 10876336, 10876337, 10876338, 10876339, 10876340, 10876341, 10876342, 10876343, 10876344, 10876345, 10876346, 10876347, 10876348, 10876349, 10876350, 10876351, 10876352, 10876353, 10876354, 10876355, 10876356, 10876357, 10876358, 10876359, 10876360, 10876361, 10876362, 10876363, 10876364, 10876365, 10876366, 10876367, 10876368, 10876369, 10876370, 10876371, 10876372, 10876373, 10876374, 10876375, 10876376, 10876377, 10876378, 10876379, 10876380, 10876381, 10876382, 10876383, 10876384, 10876385, 10876386, 10876387, 10876388, 10876389, 10876390, 10876391, 10876392, 10876393, 10876394, 10876395, 10876396, 10876397, 10876398, 10876399, 10876400, 10876401, 10876402, 10876403, 10876404, 10876405, 10876406, 10876407, 10876408, 10876409, 10876410, 10876411, 10876412, 10876413, 10876414, 10876415, 10876416, 10876417, 10876418, 10876419, 10876420, 10876421, 10876422, 10876423, 10876424, 10876425, 10876426, 10876427, 10876428, 10876429, 10876430, 10876431, 10876432, 10876433, 10876434, 10876435, 10876436, 10876437, 10876438, 10876439, 10876440, 10876441, 10876442, 10876443, 10876444, 10876445, 10876446, 10876447, 10876448, 10876449, 10876450, 10877309, 10877310, 10877311, 10877312, 10877313, 10877314, 10877315, 10877316, 10877317, 10877318, 10877319, 10877320, 10877321, 10877322, 10877323, 10877324, 10877325, 10877326, 10877327, 10877328, 10877329, 10877330, 10877333, 10877334, 10877335, 10877337, 10877338, 10877339, 10877340, 10877342, 10877344, 10877345, 10877346, 10877347, 10877348, 10877349, 10877350, 10877351, 10877353, 10877354, 10877355, 10877356, 10877357, 10877358, 10877359, 10877360, 10877362, 10877363, 10877364, 10877365, 10877366, 10877367, 10877368, 10877369, 10877370, 10877371, 10877372, 10877373, 10877374, 10877375, 10877376, 10877377, 10877378, 10877379, 10877380, 10877381, 10877382, 10877383, 10877384, 10877385, 10877386, 10877387, 10877388, 10877389, 10877390, 10877391, 10877392, 10877393, 10877394, 10877395, 10877396, 10877397, 10877398, 10877399, 10877400, 10877401, 10877402, 10877403, 10877404, 10877405, 10877406, 10877407, 10877408, 10877409, 10877410, 10877411, 10877412, 10877413, 10877498, 10877500, 10877501, 10877502, 10877513, 10877514, 10877515, 10877516, 10877521, 10877522, 10877523, 10877524, 10877525, 10877526, 10877527, 10877528, 10877529, 10877530, 10877531, 10877532, 10877533, 10877534, 10877535, 10877536, 10877611, 10877614, 10877616, 10877617, 10877618, 10877619, 10877620, 10877621, 10877622, 10877623, 10877624, 10877625, 10877626, 10877627, 10877628, 10877629, 10877630, 10877631, 10877633, 10877635, 10877636, 10877637, 10877638, 10877643, 10877644, 10877645, 10877646, 10877647, 10877648, 10877649, 10877650, 10877651, 10877652, 10877653, 10877654, 10877655, 10877656, 10877657, 10877661, 10877662, 10877663, 10877664, 10877666, 10877667, 10877668, 10877669, 10877674, 10877675, 10877676, 10877677, 10877678, 10877679, 10877680, 10877681, 10877682, 10877683, 10877684, 10877685, 10877686, 10877687, 10877688, 10877689, 10877690, 10877691, 10877692, 10877693, 10877694, 10877695, 10877696, 10877697, 10877698, 10877699, 10877700, 10877701, 10877702, 10877703, 10877704, 10877705, 10877706, 10877707, 10877708, 10877709, 10877710, 10877711, 10877712, 10877713, 10877714, 10877715, 10877716, 10877717, 10877718, 10877719, 10877720, 10877721, 10877722, 10877723, 10877724, 10877725, 10877726, 10877727, 10877728, 10877729, 10877730, 10877731, 10877732, 10877733, 10877734, 10877735, 10877736, 10877737, 10877738, 10877739, 10877740, 10877741, 10877742, 10877743, 10877744, 10877745, 10877746, 10877747, 10877748, 10877749, 10877750, 10877751, 10877752, 10877753, 10877754, 10877755, 10877756, 10877757, 10877758, 10877759, 10877760, 10877761, 10877762, 10877763, 10877764, 10877765, 10877766, 10877767, 10877768, 10877769, 10877770, 10877771, 10877772, 10877773, 10877774, 10877775, 10877776, 10877777, 10877778, 10877779, 10877780, 10877781, 10877782, 10877783, 10877784, 10877785, 10877786, 10877787, 10877788, 10877789, 10877790, 10877791, 10877792, 10877793, 10877794, 10877795, 10877796, 10877797, 10877805, 10877807, 10877820, 10877821, 10877823, 10877824, 10877825, 10877826, 10877827, 10877828, 10877829, 10877830, 10877831, 10877837, 10877838, 10877839, 10877840, 10877842, 10877845, 10877851, 10877852, 10877853, 10877854, 10877855, 10877856, 10877857, 10878109, 10878110, 10878111, 10878112, 10878113, 10878114, 10878115, 10878116, 10878117, 10878118, 10878119, 10878120, 10878377, 10878378, 10878379, 10878381, 10878382, 10878383, 10878384, 10878385, 10878386, 10878387, 10878388, 10878389, 10878390, 10878391, 10878392, 10878393, 10878395, 10878396, 10878397, 10878398, 10878399, 10878400, 10878401, 10878402, 10878403, 10878404, 10878429, 10878434, 10878435, 10878436, 10878437, 10878438, 10878439, 10878440, 10878441, 10878442, 10878443, 10878444, 10878445, 10878446, 10878447, 10878448, 10878449, 10878450, 10878452, 10878453, 10878454, 10878455, 10878515, 10878516, 10878518, 10878575, 10878576, 10878577, 10878578, 10878579, 10878580, 10878581, 10878582, 10878583, 10878584, 10878585, 10878586, 10878587, 10878588, 10878589, 10878590, 10878591, 10878592, 10878593, 10878594, 10878595, 10878596, 10878597, 10878598, 10878599, 10878600, 10878601, 10878602, 10878772, 10878773, 10878774, 10878775, 10878776, 10878777, 10878778, 10878779, 10878780, 10878781, 10878837, 10878838, 10878839, 10878840, 10878841, 10878842, 10878843, 10878844, 10878900, 10878901, 10878902, 10878903, 10878904, 10878905, 10878906, 10878907, 10878910, 10878911, 10878912, 10878913, 10878914, 10878915, 10878916, 10878917, 10878918, 10878919, 10878920, 10878921, 10878922, 10878923, 10878924, 10878925, 10878926, 10878927, 10878928, 10878929, 10878930, 10878931, 10878932, 10878933, 10878934, 10878935, 10878936, 10878937, 10878938, 10878939, 10878940, 10878941, 10878942, 10878943, 10878944, 10878945, 10878959, 10878960, 10878961, 10878962, 10878963, 10878964, 10878965, 10878966, 10878967, 10878968, 10878969, 10878974, 10878977, 10878978, 10878979, 10878980, 10878981, 10878983, 10878988, 10878989, 10878990, 10878991, 10878992, 10878993, 10878994, 10878995, 10878996, 10880420, 10880423, 10880424, 10880510, 10880511, 10880512, 10880513, 10880514, 10880515, 10880516, 10880517, 10880518, 10880519, 10880520, 10880521, 10880522, 10880523, 10880524, 10880525, 10880526, 10880527, 10880528, 10880529, 10880530, 10880531, 10880532, 10880533, 10880534, 10880535, 10880536, 10880537, 10880538, 10880539, 10880540, 10880541, 10880542, 10880543, 10880544, 10880545, 10880546, 10880547, 10880548, 10880549, 10880550, 10880551, 10880552, 10880553, 10880554, 10880555, 10880556, 10880557, 10880558, 10880559, 10880560, 10880561, 10880562, 10880563, 10880564, 10880565, 10880566, 10880567, 10880568, 10880569, 10880570, 10880571, 10880572, 10880573, 10880574, 10880575, 10880576, 10880577, 10880578, 10880579, 10880580, 10880581, 10880590, 10880591, 10880592, 10880593, 10880594, 10880595, 10880596, 10880597, 10880598, 10880599, 10880600, 10880601, 10880602, 10880603, 10880604, 10880605, 10880606, 10880607, 10880608, 10880609, 10880610, 10880611, 10880612, 10880613, 10880614, 10880615, 10880616, 10880617, 10880618, 10880619, 10880620, 10880621, 10880622, 10880623, 10880624, 10880625, 10880626, 10880627, 10880628, 10880629, 10880630, 10880631, 10880632, 10880633, 10880634, 10880635, 10880636, 10880637, 10880638, 10880639, 10880640, 10880643, 10880644, 10880646, 10880647, 10880657, 10880658, 10880659, 10880660, 10880661, 10880662, 10880663, 10880664, 10880665, 10880666, 10880667, 10880668, 10880669, 10880670, 10880671, 10880672, 10880673, 10880674, 10880675, 10880676, 10880677, 10880678, 10880679, 10880680, 10880681, 10880683, 10880684, 10880685, 10880686, 10880687, 10880688, 10880689, 10880690, 10880691, 10880692, 10880693, 10880694, 10880695, 10880696, 10880697, 10880698, 10880699, 10880700, 10880701, 10880702, 10880703, 10880704, 10880705, 10880706, 10880707, 10880708, 10880709, 10880710, 10880711, 10880712, 10880713, 10880714, 10880715, 10880716, 10880717, 10880718, 10880719, 10880720, 10880721, 10880722, 10880723, 10880725, 10880726, 10880727, 10880728, 10880729, 10880730, 10880731, 10880732, 10880733, 10880734, 10880735, 10880736, 10880737, 10880738, 10880739, 10880740, 10880741, 10880742, 10880743, 10880744, 10880745, 10880746, 10880747, 10880748, 10880749, 10880750, 10880751, 10880752, 10880753, 10880754, 10880755, 10880756, 10880757, 10880758, 10880759, 10880760, 10880761, 10880762, 10880763, 10880764, 10880765, 10880766, 10880767, 10880768, 10880769, 10880770, 10880771, 10880772, 10880773, 10880774, 10880775, 10880776, 10880777, 10880778, 10880779, 10880780, 10880781, 10880782, 10880783, 10880784, 10880785, 10880786, 10880787, 10880788, 10882775, 10882776, 10882777, 10882778, 10882779, 10882780, 10882781, 10882782, 10882783, 10882784, 10882785, 10882786, 10882787, 10882788, 10882789, 10882790, 10882791, 10882792, 10882793, 10882794, 10882795, 10882796, 10882797, 10882798, 10882799, 10882800, 10882801, 10882802, 10882803, 10882804, 10882805, 10882806, 10882807, 10882808, 10882809, 10882810, 10882811, 10882812, 10882813, 10882814, 10882815, 10882816, 10882817, 10882818, 10882819, 10882820, 10882821, 10882822, 10882823, 10882824, 10882825, 10882826, 10882827, 10882828, 10882829, 10882830, 10882831, 10882832, 10882833, 10882834, 10882835, 10882836, 10882837, 10882838, 10882839, 10882840, 10882841, 10882842, 10882843, 10882844, 10882845, 10882846, 10882847, 10882848, 10882849, 10882850, 10882851, 10882852, 10882853, 10882854, 10882855, 10882856, 10882857, 10882858, 10882859, 10882860, 10882861, 10882862, 10882863, 10882864, 10882865, 10882866, 10882867, 10882868, 10882869, 10882870, 10882871, 10882872, 10882873, 10882874, 10882875, 10882876, 10882877, 10882894, 10882895, 10882896, 10882897, 10882898, 10882899, 10882900, 10882901, 10882902, 10882903, 10882904, 10882905, 10882906, 10882907, 10882908, 10882909, 10882910, 10882911, 10882912, 10882913, 10882914, 10882915, 10882916, 10882917, 10882918, 10882919, 10882920, 10882921, 10882922, 10882923, 10882924, 10882925, 10882927, 10882928, 10882929, 10882930, 10882931, 10882932, 10882933, 10882934, 10882935, 10882936, 10882937, 10882938, 10882939, 10882940, 10882941, 10882942, 10882943, 10882944, 10882945, 10882946, 10882947, 10882948, 10882949, 10882950, 10882951, 10882952, 10882953, 10882954, 10882955, 10882956, 10882957, 10882958, 10882959, 10882960, 10882961, 10882962, 10882963, 10882964, 10882965, 10882966, 10882967, 10882968, 10882969, 10882970, 10882971, 10882972, 10882973, 10882974, 10882975, 10882976, 10882977, 10882978, 10882979, 10882980, 10882981, 10882982, 10882983, 10882984, 10882985, 10882986, 10882987, 10882988, 10882989, 10882990, 10882991, 10882992, 10882993, 10882994, 10882995, 10882996, 10882997, 10882998, 10882999, 10883000, 10883001, 10883002, 10883003, 10883004, 10883005, 10883006, 10883007, 10883008, 10883009, 10883010, 10883011, 10883012, 10883013, 10883014, 10883015, 10883016, 10883017, 10883018, 10883019, 10883020, 10883021, 10883022, 10883023, 10883024, 10883025, 10883026, 10883027, 10883028, 10883029, 10883030, 10883032, 10883033, 10883034, 10883035, 10883036, 10883037, 10883038, 10883040, 10883041, 10883042, 10883043, 10883044, 10883045, 10883046, 10883047, 10883048, 10883049, 10883050, 10883051, 10883052, 10883053, 10883054, 10883055, 10883056, 10883057, 10883058, 10883059, 10883060, 10883061, 10883062, 10883063, 10883064, 10883065, 10883078, 10883101, 10883102, 10883103, 10883104, 10883105, 10883106, 10883107, 10883108, 10883109, 10883110, 10883111, 10883112, 10883113, 10883114, 10883115, 10883116, 10883117, 10883118, 10883119, 10883120, 10883121, 10883122, 10883123, 10883124, 10883125, 10883126, 10883127, 10883128, 10883129, 10883130, 10883131, 10883132, 10883133, 10883134, 10883135, 10883136, 10883137, 10883138, 10883139, 10883140, 10883141, 10883142, 10883143, 10883144, 10883145, 10883146, 10883147, 10883148, 10883149, 10883150, 10883151, 10883152, 10883153, 10883154, 10883155, 10883156, 10883157, 10883158, 10883159, 10883160, 10883161, 10883162, 10883163, 10883164, 10883165, 10883166, 10883167, 10883168, 10883169, 10883170, 10883171, 10883172, 10883174, 10883175, 10883176, 10883177, 10883178, 10883179, 10883180, 10883181, 10883182, 10883183, 10883211, 10883212, 10883213, 10883214, 10883215, 10883216, 10883217, 10883218, 10883219, 10883220, 10883221, 10883223, 10883224, 10883226, 10883227, 10883229, 10883230, 10883231, 10883232, 10883234, 10883235, 10883238, 10883239, 10883241, 10883242, 10883243, 10883244, 10883248, 10883249, 10883250, 10883251, 10883252, 10883253, 10883254, 10883255, 10883256, 10883258, 10883259, 10883260, 10883261, 10883262, 10883263, 10883266, 10883267, 10883268, 10883269, 10883270, 10883271, 10883272, 10883273, 10883275, 10883276, 10883277, 10883278, 10883279, 10883280, 10883281, 10883282, 10883283, 10883284, 10883286, 10883287, 10883288, 10883289, 10883290, 10883291, 10883292, 10883293, 10883294, 10883295, 10883296, 10883297, 10883298, 10883299, 10883300, 10883301, 10883302, 10883303, 10883305, 10883306, 10883308, 10883309, 10883310, 10883311, 10883314, 10883315, 10883317, 10883319, 10883320, 10883321, 10883322, 10883323, 10883324, 10883325, 10883326, 10883327, 10883328, 10883329, 10883330, 10883331, 10883332, 10883333, 10883334, 10883335, 10883336, 10883337, 10883338, 10883339, 10883340, 10883341, 10883342, 10883343, 10883344, 10883345, 10883346, 10883347, 10883348, 10883349, 10883350, 10883351, 10883352, 10883354, 10883356, 10883357, 10883358, 10883359, 10883360, 10883361, 10883362, 10883363, 10883365, 10883366, 10883367, 10883368, 10883369, 10883372, 10883373, 10883374, 10883375, 10883404, 10883406, 10883407, 10883408, 10883409, 10883410, 10883411, 10883412, 10883413, 10883414, 10883415, 10883416, 10883417, 10883418, 10883419, 10883420, 10883421, 10883422, 10883423, 10883424, 10883425, 10883426, 10883427, 10883428, 10883429, 10883430, 10883431, 10883432, 10883433, 10883434, 10883435, 10883436, 10883437, 10883438, 10883439, 10883440, 10883441, 10883442, 10883443, 10883444, 10883445, 10883446, 10883447, 10883448, 10883449, 10883450, 10883451, 10883452, 10883454, 10883455, 10883456, 10883457, 10883458, 10883459, 10883460, 10883461, 10883462, 10883464, 10883465, 10883466, 10883467, 10883468, 10883469, 10883470, 10883471, 10883472, 10883473, 10883474, 10883475, 10883476, 10883477, 10883478, 10883479, 10883480, 10883481, 10883482, 10883483, 10883494, 10883495, 10883496, 10883497, 10883498, 10883499, 10883500, 10883501, 10883502, 10883503, 10883504, 10883505, 10883506, 10883507, 10883508, 10883509, 10883510, 10883511, 10883512, 10883513, 10883514, 10883515, 10883516, 10883517, 10883518, 10883519, 10883520, 10883521, 10883532, 10883533, 10883534, 10883535, 10883536, 10883537, 10883927, 10883928, 10883929, 10883930, 10883931, 10883932, 10883933, 10883934, 10883935, 10883937, 10883938, 10883939, 10883940, 10883942, 10883943, 10883944, 10883945, 10883947, 10883951, 10883952, 10883953, 10883954, 10883955, 10883956, 10883957, 10883958, 10883959, 10883961, 10883962, 10883963, 10883964, 10883965, 10883966, 10883967, 10883968, 10883969, 10883970, 10883972, 10883973, 10883974, 10883977, 10883978, 10883979, 10883981, 10883983, 10883984, 10883985, 10883986, 10883987, 10883988, 10883989, 10883990, 10883991, 10883992, 10883993, 10883994, 10883997, 10883998, 10883999, 10884000, 10884002, 10884003, 10884004, 10884008, 10884009, 10884010, 10884011, 10884012, 10884013, 10884014, 10884015, 10884016, 10884017, 10884018, 10884019, 10884020, 10884021, 10884022, 10884024, 10884026, 10884027, 10884028, 10884029, 10884030, 10884031, 10884032, 10884033, 10884034, 10884035, 10884036, 10884037, 10884038, 10884039, 10884040, 10884041, 10884042, 10884043, 10884045, 10884046, 10884047, 10884048, 10884049, 10884050, 10884051, 10884052, 10884053, 10884054, 10884055, 10884056, 10884057, 10884058, 10884060, 10884061, 10884062, 10884063, 10884064, 10884065, 10884066, 10884067, 10884068, 10884069, 10884070, 10884071, 10884072, 10884073, 10884074, 10884075, 10884076, 10884077, 10884079, 10884080, 10884081, 10884082, 10884083, 10884084, 10884085, 10884086, 10884087, 10884088, 10884089, 10884090, 10884091, 10884092, 10884093, 10884094, 10884095, 10884096, 10884097, 10884098, 10884099, 10884100, 10884101, 10884102, 10884103, 10884104, 10884105, 10884106, 10884107, 10884108, 10884109, 10884110, 10884111, 10884113, 10884114, 10884115, 10884116, 10884117, 10884119, 10884120, 10884121, 10884123, 10884124, 10884126, 10884128, 10884129, 10884130, 10884131, 10884132, 10884134, 10884139, 10884140, 10884143, 10884144, 10884146, 10884147, 10884148, 10884149, 10884150, 10884151, 10884154, 10884155, 10884157, 10884159, 10884160, 10884162, 10884163, 10884168, 10884169, 10884170, 10884173, 10884174, 10884175, 10884176, 10884177, 10884178, 10884179, 10884182, 10884183, 10884184, 10884186, 10884187, 10884188, 10884189, 10884190, 10884191, 10884192, 10884193, 10884194, 10884195, 10884196, 10884197, 10884198, 10884199, 10884200, 10884201, 10884202, 10884203, 10884204, 10884205, 10884206, 10884207, 10884208, 10884209, 10884210, 10884211, 10884212, 10884213, 10884214, 10884215, 10884216, 10884217, 10884218, 10884219, 10884220, 10884221, 10884222, 10884223, 10884224, 10884225, 10884226, 10884227, 10884228, 10884229, 10884230, 10884231, 10884232, 10884233, 10884234, 10884235, 10884236, 10884237, 10884238, 10884240, 10884241, 10884243, 10884244, 10884245, 10884246, 10884247, 10884248, 10884249, 10884250, 10884252, 10884254, 10884256, 10884257, 10884258, 10884260, 10884261, 10884262, 10884263, 10884264, 10884265, 10884266, 10884267, 10884268, 10884269, 10884271, 10884274, 10884275, 10884277, 10884278, 10884279, 10884280, 10884281, 10884282, 10884283, 10884284, 10884285, 10884286, 10884287, 10884288, 10884289, 10884290, 10884291, 10884292, 10884293, 10884295, 10884296, 10884299, 10884300, 10884301, 10884302, 10884303, 10884304, 10884305, 10884307, 10884308, 10884309, 10884310, 10884311, 10884313, 10884314, 10884315, 10884317, 10884318, 10884319, 10884320, 10884321, 10884322, 10884323, 10884324, 10884325, 10884326, 10884327, 10884328, 10884329, 10884330, 10884331, 10884332, 10884333, 10884334, 10884335, 10884336, 10884337, 10884338, 10884339, 10884340, 10884341, 10884342, 10884345, 10884346, 10884347, 10884348, 10884349, 10884350, 10884351, 10884352, 10884353, 10884354, 10884355, 10884356, 10884357, 10884358, 10884359, 10884360, 10884361, 10884362, 10884363, 10884365, 10884366, 10884367, 10884369, 10884371, 10884374, 10884375, 10884376, 10884377, 10884378, 10884379, 10884380, 10884381, 10884382, 10884383, 10884384, 10884385, 10884386, 10884387, 10884388, 10884389, 10884390, 10884391, 10884392, 10885666, 10885668, 10885669, 10885670, 10885671, 10885672, 10885673, 10885674, 10885675, 10885676, 10885677, 10885678, 10885679, 10885680, 10885681, 10885682, 10885683, 10885684, 10885685, 10885686, 10885687, 10885688, 10885689, 10885690, 10885691, 10885692, 10885693, 10885694, 10885695, 10885696, 10885697, 10885698, 10885699, 10885700, 10885701, 10885703, 10885704, 10885705, 10885706, 10885707, 10885708, 10885709, 10885711, 10885712, 10885713, 10885714, 10885716, 10885717, 10885718, 10885719, 10885720, 10885721, 10885722, 10885724, 10885726, 10885728, 10885730, 10885731, 10885732, 10885733, 10885734, 10885736, 10885738, 10885739, 10885740, 10885741, 10885745, 10885746, 10885747, 10885748, 10885749, 10885750, 10885751, 10885752, 10885754, 10885755, 10885756, 10885757, 10885758, 10885759, 10885760, 10885761, 10885762, 10885763, 10885764, 10885765, 10885767, 10885768, 10885769, 10885770, 10885771, 10885773, 10885774, 10885776, 10885778, 10885779, 10885859, 10885860, 10885861, 10885862, 10885863, 10885864, 10885865, 10885866, 10885867, 10885868, 10885869, 10885870, 10885871, 10885872, 10885873, 10885874, 10885875, 10885876, 10885877, 10885878, 10885879, 10885880, 10885881, 10885882, 10885883, 10885884, 10885888, 10885893, 10885900, 10885902, 10885905, 10885907, 10885909, 10885910, 10885911, 10885912, 10885913, 10885915, 10885916, 10885917, 10885918, 10885919, 10885920, 10885923, 10885924, 10885926, 10885927, 10885928, 10885930, 10885932, 10885934, 10885936, 10885937, 10885938, 10885939, 10885940, 10885943, 10885950, 10885953, 10885956, 10885957, 10885959, 10885960, 10885962, 10885964, 10885966, 10885970, 10885971, 10885976, 10885978, 10885980, 10885981, 10885983, 10885984, 10885990, 10885991, 10885992, 10885995, 10885997, 10885998, 10885999, 10886000, 10886001, 10886002, 10886003, 10886004, 10886005, 10886006, 10886007, 10886008, 10886009, 10886010, 10886011, 10886012, 10886013, 10886016, 10886017, 10886018, 10886019, 10886020, 10886021, 10886022, 10886023, 10886024, 10886025, 10886026, 10886027, 10886028, 10886029, 10886030, 10886031, 10886032, 10886034, 10886035, 10886036, 10886039, 10886040, 10886041, 10886043, 10886045, 10886046, 10886047, 10886049, 10886050, 10886051, 10886052, 10886053, 10886054, 10886055, 10886056, 10886058, 10886059, 10886060, 10886062, 10886063, 10886064, 10886065, 10886066, 10886067, 10886068, 10886069, 10886070, 10886071, 10886073, 10886074, 10886075, 10886076, 10886077, 10886078, 10886080, 10886082, 10886083, 10886086, 10886087, 10886088, 10886089, 10886090, 10886093, 10886094, 10886097, 10886098, 10886101, 10886103, 10886105, 10886107, 10886109, 10886111, 10886117, 10886118, 10886119, 10886120, 10886121, 10886122, 10886123, 10886124, 10886125, 10886126, 10886127, 10886128, 10886129, 10886130, 10886131, 10886132, 10886133, 10886134, 10886135, 10886136, 10886137, 10886138, 10886139, 10886140, 10886141, 10886142, 10886143, 10886144, 10886145, 10886146, 10886147, 10886148, 10886149, 10886150, 10886151, 10886152, 10886153, 10886154, 10886155, 10886156, 10886158, 10886161, 10886166, 10886167, 10886168, 10886169, 10886170, 10886171, 10886172, 10886173, 10886174, 10886175, 10886176, 10886177, 10886178, 10886180, 10886181, 10886182, 10886185, 10886186, 10886187, 10886188, 10886189, 10886191, 10886193, 10886197, 10886198, 10886199, 10886201, 10886203, 10886204, 10886209, 10886210, 10886211, 10886212, 10886214, 10886215, 10886217, 10886218, 10886220, 10886221, 10886222, 10886223, 10886224, 10886225, 10886226, 10886230, 10886233, 10886235, 10886236, 10886238, 10886239, 10886240, 10886241, 10886242, 10886244, 10886248, 10886249, 10886252, 10886253, 10886254, 10886255, 10886258, 10886259, 10886260, 10908959, 10908980, 10908988, 10909044, 10911747, 10911749, 10911750, 10911754, 10911755, 10911756, 10911759, 10911775, 10911776, 10911777, 10911786, 10911787, 10911788, 10911789, 10911790, 10911792, 10912096, 10912112, 10912113, 10912119, 10912120, 10912126, 10912127, 10912128, 10912130, 10912210, 10912213, 10912215, 10912216, 10912217, 10912218, 10912219, 10912220, 10912221, 10912222, 10912223, 10912284, 10912285, 10912286, 10912287, 10912288, 10912289, 10912290, 10912291, 10912292, 10912293, 10912294, 10912295, 10912296, 10912297, 10912298, 10912304, 10912305, 10912306, 10912307, 10912308, 10912309, 10912310, 10912311, 10912312, 10912313, 10912314, 10912315, 10912316, 10912317, 10912644, 10912645, 10912646, 10912647, 10912648, 10912649, 10912650, 10912651, 10912652, 10912653, 10912654, 10912655, 10912656, 10912657, 10912658, 10912659, 10912660, 10912661, 10912703, 10912704, 10912705, 10912706, 10912707, 10912708, 10912709, 10912710, 10912711, 10912712, 10912713, 10912714, 10912715, 10912716, 10912717, 10912718, 10912719, 10912720, 10912721, 10912722, 10912723, 10920314, 10920315, 10920316, 10920317, 10920318, 10920319, 10920320, 10920321, 10920322, 10920323, 10920324, 10920325, 10920326, 10920327, 10920328, 10920329, 10920330, 10920331, 10920332, 10920333, 10920334, 10920335, 10920336, 10920337, 10920338, 10920339, 10920340, 10920341, 10920342, 10920343, 10920344, 10920362, 10920363, 10920365, 10920368, 10920369, 10920370, 10920402, 10920404, 10920405, 10920408, 10920409, 10920410, 10920413, 10920414, 10920416, 10920427, 10920428, 10920429, 10920430, 10920431, 10920433, 10920434, 10920435, 10920436, 10920437, 10920438, 10920440, 10920442, 10920448, 10920450, 10920451, 10920453, 10920454, 10920455, 10920456, 10920465, 10920473, 10920476, 10920478, 10920479, 10920480, 10920481, 10920482, 10920483, 10920581, 10920583, 10920584, 10920585, 10920586, 10920587, 10920589, 10920592, 10920593, 10920594, 10920596, 10920597, 10920598, 10920599, 10920600, 10920601, 10920602, 10920603, 10920604, 10920605, 10920606, 10920607, 10920608, 10920609, 10920610, 10920611, 10920612, 10920613, 10920614, 10920615, 10920616, 10920618, 10920619, 10920620, 10920621, 10920622, 10920624, 10920625, 10920626, 10920627, 10920628, 10920629, 10920632, 10920633, 10920634, 10920635, 10920636, 10920637, 10920638, 10920639, 10920642, 10920643, 10920644, 10920645, 10920646, 10920647, 10920648, 10920649, 10920650, 10920651, 10920652, 10921613, 10921614, 10940625, 10940626, 10940627, 10940628, 10940629, 10952497, 10952506, 10953402, 10954950, 10954951, 10954952, 10954953, 10954981, 10954984, 10954985, 10954986, 10954988, 10954990, 10954992, 10955318, 10955331, 10955442, 10955455, 10955468, 10955486, 10955538, 10955539, 10955541, 10955542, 10955543, 10955545, 10955548, 10955549, 10955568, 10955573, 10960679, 10960680, 10960681, 10960718, 10963531, 10963532, 10963533, 10963541, 10963542, 10963543, 10963544, 10963545, 10963546, 10963547, 10963548, 10963551, 10963553, 10963554, 10963564, 10963565, 10963566, 10963567, 10963568, 10963570, 10963571, 10963572, 10963573, 10963574, 10963575, 10963580, 10963581, 10963582, 10963584, 10963585, 10963586, 10963587, 10963588, 10963589, 10963592, 10963595, 10963596, 10963598, 10963599, 10963600, 10963602, 10963604, 10963605, 10963607, 10963609, 10963610, 10963611, 10963612, 10963614, 10963615, 10963617, 10963619, 10963620, 10963623, 10963624, 10963625, 10963626, 10963627, 10963629, 10963630, 10963631, 10963632, 10963633, 10963634, 10963635, 10963636, 10963637, 10963638, 10963639, 10963640, 10963641, 10963642, 10963643, 10963644, 10963646, 10963647, 10963648, 10963649, 10963650, 10963652, 10963653, 10963656, 10963658, 10963660, 10963661, 10963663, 10963666, 10963668, 10963670, 10963672, 10963673, 10963674, 10963676, 10963678, 10963679, 10963681, 10963682, 10963683, 10963684, 10963685, 10963686, 10963687, 10963688, 10963689, 10963690, 10963691, 10963692, 10963693, 10963694, 10963695, 10963697, 10963698, 10963699, 10963701, 10963702, 10963703, 10963705, 10963709, 10963710, 10963711, 10963713, 10963715, 10963717, 10963718, 10963719, 10963720, 10963727, 10963729, 10963730, 10963731, 10963732, 10963748, 10963749, 10992386, 10992387, 10992388, 10992390, 10992391, 10992392, 10992393, 10992395, 10992396, 10992397, 10992398, 10992399, 10992400, 10992403, 10992404, 10992405, 10992406, 10992407, 10992408, 10992409, 10992410, 10992411, 10992413, 10992414, 10992415, 10992461, 10992462, 10992463, 10992464, 10992465, 10992466, 10992467, 10992468, 10992469, 10992470, 10992471, 10992472, 10992473, 10992474, 10992475, 10992483, 10992484, 10992485, 10992486, 10992487, 10992488, 10992489, 10992490, 10992491, 10992492, 10992493, 10992494, 10992495, 10992496, 10992497, 10992498, 10992499, 10992500, 10992501, 10992502, 10992503, 10992504, 10992505, 10992506, 10992507, 10992508, 10992509, 10992510, 10992511, 10992512, 10992513, 10875374, 10875375, 10875378, 10881077, 10881078, 10881079, 10881080, 10881081, 10881084, 10881200, 10881201, 10881202, 10881207, 10881213, 10881269, 10881271, 10881304, 10881306, 10881334, 10881794, 10881795, 10881796, 10881797, 10881799, 10882146, 10882147, 10882148, 10882149, 10882150, 10882151, 10882152, 10882190, 10882191, 10882192, 10882193, 10882194, 10882195, 10882361, 10882362, 10882363, 10882647, 10882648, 10882649, 10882650, 10883812, 10883814, 10883837, 10883838, 10883840, 10883852, 10883905, 10883906, 10883907, 10883908, 10883909, 10883914, 10883915, 10884419, 10884420, 10884425, 10884426, 10884440, 10884442, 10884443, 10884444, 10884445, 10884447, 10884449, 10884450, 10884451, 10884453, 10884458, 10884459, 10884460, 10884461, 10884464, 10884465, 10884477, 10884482, 10884483, 10884484, 10884501, 10884502, 10884519, 10884521, 10884539, 10884540, 10884557, 10884558, 10884559, 10884560, 10884562, 10884563, 10884564, 10884565, 10884566, 10884567, 10884568, 10884569, 10884570, 10884572, 10884573, 10884576, 10884577, 10884578, 10884579, 10884580, 10884581, 10884582, 10884583, 10884584, 10884585, 10884586, 10884587, 10884588, 10884589, 10884590, 10884591, 10884592, 10884594, 10884596, 10884597, 10884598, 10884599, 10884600, 10884601, 10884604, 10974334, 10974782, 10924811, 10883788, 10883789, 10883790, 10883795, 10883797, 10883798, 10883799, 10883800, 10883803, 10883804, 10883805, 10883806, 10883807, 10883808, 10883809, 10883810, 10883924, 10883925, 10883926, 10884409, 10884410, 10884411, 10884412, 10884413, 10884414, 10886262, 10886263, 10886264, 10886265, 10886266, 10886267, 10886268, 10886269, 10886270, 10886271, 10886272, 10886273, 10886274, 10886275, 10886276, 10886277, 10886278, 10886279, 10886280, 10886281, 10886283, 10886284, 10886285, 10886286, 10886287, 10886288, 10886290, 10886291, 10886292, 10886293, 10886294, 10886295, 10886296, 10886297, 10886298, 10886299, 10886300, 10886301, 10886302, 10886303, 10886304, 10886305, 10886306, 10886307, 10886308, 10886309, 10886310, 10886311, 10886312, 10886313, 10886314, 10886315, 10886316, 10886317, 10886318, 10886319, 10886320, 10886321, 10886322, 10886323, 10886324, 10886325, 10886326, 10886327, 10886328, 10886329, 10886330, 10886331, 10886332, 10886333, 10886334, 10886335, 10886336, 10886337, 10886338, 10886339, 10886340, 10886341, 10886342, 10886343, 10886344, 10886345, 10886346, 10886347, 10886348, 10886349, 10886350, 10886351, 10886352, 10911778, 10911780, 10911782, 10911783, 10911784, 10911785, 10911794, 10911795, 10911796, 10911797, 10911798, 10911799, 10912892, 10913009, 10913024, 10913025, 10913026, 10913027, 10913028, 10913029, 10913030, 10913037, 10913038, 10913039, 10913040, 10913041, 10913042, 10913043, 10913044, 10913045, 10913123, 10913124, 10913125, 10913126, 10913127, 10913128, 10913129, 10913133, 10913134, 10913135, 10913136, 10913137, 10913138, 10913139, 10913140, 10913141, 10913142, 10913143, 10913144, 10913145, 10913146, 10913147, 10913148, 10913149, 10913150, 10913208, 10913209, 10913210, 10913211, 10913212, 10913213, 10913214, 10913215, 10913216, 10913217, 10913218, 10913219, 10913220, 10913221, 10913222, 10913223, 10913224, 10913225, 10913385, 10913404, 10913412, 10913416, 10913430, 10913432, 10913434, 10913435, 10913468, 10913469, 10913470, 10913475, 10913476, 10913477, 10913478, 10913479, 10913530, 10913531, 10913532, 10913533, 10913534, 10913535, 10913536, 10913537, 10913538, 10913539, 10913540, 10913541, 10913593, 10913594, 10913595, 10913596, 10913597, 10913598, 10913599, 10913600, 10913601, 10913602, 10913603, 10913604, 10913605, 10913606, 10913607, 10913608, 10913609, 10913610, 10913611, 10913612, 10913613, 10913614, 10913619, 10913621, 10913622, 10913623, 10913917, 10913923, 10913928, 10913947, 10913948, 10913949, 10913950, 10913951, 10913952, 10913953, 10913954, 10913955, 10913956, 10913957, 10913958, 10913959, 10913960, 10913961, 10913962, 10913963, 10913964, 10913965, 10913966, 10913967, 10913968, 10913969, 10913970, 10913971, 10913972, 10913973, 10913974, 10913975, 10913976, 10913977, 10913978, 10915800, 10915801, 10915802, 10915803, 10915804, 10915805, 10915806, 10915807, 10915808, 10915809, 10915810, 10915811, 10915812, 10915813, 10915814, 10915815, 10915816, 10915817, 10915818, 10915819, 10915820, 10915821, 10915822, 10915823, 10915824, 10915825, 10915826, 10915827, 10915828, 10916369, 10916370, 10916371, 10916372, 10916373, 10916374, 10916375, 10916376, 10916377, 10916378, 10916379, 10916380, 10916381, 10916382, 10916383, 10916384, 10916385, 10916386, 10916387, 10916388, 10916389, 10916390, 10916391, 10916392, 10916393, 10916394, 10916395, 10916396, 10916397, 10916398, 10916399, 10916400, 10916401, 10916402, 10916403, 10916404, 10916405, 10916406, 10916407, 10916408, 10916409, 10916410, 10917384, 10917385, 10917386, 10917387, 10917388, 10917389, 10917390, 10917396, 10920664, 10920665, 10924079, 10924080, 10924081, 10924082, 10924083, 10924084, 10924085, 10924086, 10924087, 10924088, 10924089, 10924090, 10924091, 10924092, 10924093, 10924094, 10924095, 10924096, 10924097, 10924098, 10924099, 10924100, 10924101, 10924102, 10924103, 10924104, 10924105, 10924106, 10924107, 10924108, 10924109, 10924110, 10924111, 10924112, 10924113, 10924114, 10924115, 10924116, 10924117, 10924118, 10924119, 10924120, 10924121, 10924122, 10928349, 10928350, 10928351, 10928352, 10928353, 10928354, 10928355, 10928356, 10928357, 10928358, 10928359, 10934311, 10934313, 10934314, 10934315, 10934319, 10934320, 10934321, 10934324, 10934325, 10934326, 10934327, 10934328, 10934329, 10934456, 10933265, 11192181, 10958995, 10959022, 10959023, 10959024, 10959026, 10959027, 10959028, 10959029, 10959332, 10959373, 10959374, 10959375, 10959409, 10959412, 10959415, 10959416, 10959419, 10959420, 10959421, 10959423, 10959425, 10959426, 10959427, 10959428, 10959429, 10959430, 10959431, 10959432, 10959433, 10959434, 10959435, 10959436, 10959437, 10959449, 10959452, 10959453, 10959454, 10959455, 10959456, 10959457, 10959458, 10959460, 10959461, 10959462, 10959463, 10959464, 10959465, 10959466, 10959467, 10959468, 10959469, 10959470, 10959510, 10959511, 10959512, 10959513, 10959514, 10959515, 10959516, 10959517, 10959518, 10959573, 10959574, 10959576, 10959577, 10959579, 10959580, 10959581, 10959582, 10959606, 10959609, 10959610, 10959611, 10959612, 10959623, 10959624, 10959665, 10959669, 10959670, 10959671, 10959675, 10959676, 10959680, 10959681, 10959996, 10960055, 10960058, 10960266, 10960272, 10960510, 10960523, 10960529, 10960537, 10960538, 10960539, 10960540, 10960556, 10960557, 10963792, 10963796, 10963797, 10963798, 10963800, 10963807, 10963808, 10963818, 10963820, 10964073, 10964074, 10964075, 10964076, 10964077, 10964079, 10964080, 10964081, 10964082, 10964083, 10964084, 10964085, 10964086, 10964087, 10964090, 10964120, 10964121, 10964122, 10964123, 10964183, 10964187, 10964200, 10964216, 10964217, 10964218, 10964219, 10964220, 10964221, 10964222, 10964223, 10964224, 10964225, 10964226, 10964227, 10964228, 10964230, 10964231, 10964232, 10964233, 10964234, 10964235, 10964236, 10964237, 10964238, 10964239, 10964240, 10964241, 10964242, 10964252, 10967443, 10967444, 10967445, 10967447, 10967455, 10967505, 10967506, 10967507, 10967508, 10967509, 10967555, 10967556, 10967557, 10967560, 10967561, 10967562, 10967563, 10967564, 10967565, 10967566, 10967567, 10967568, 10967569, 10967584, 10967585, 10967586, 10967587, 10967588, 10967589, 10967590, 10967591, 10967622, 10967976, 10967977, 10967980, 10967984, 10967985, 10967992, 10967993, 10967994, 10967995, 10968260, 10968262, 10968266, 10968267, 10968272, 10968273, 10968274, 10968275, 10968276, 10968277, 10968278, 10968279, 10968280, 10968281, 10968282, 10968283, 10968284, 10968285, 10968286, 10968287, 10968288, 10968289, 10968290, 10968291, 10968292, 10968293, 10968294, 10968295, 10968296, 10968297, 10968298, 10968299, 10968300, 10968303, 10968304, 10968305, 10968306, 10968307, 10968308, 10968309, 10968310, 10968311, 10968312, 10968313, 10968314, 10968315, 10968316, 10968318, 10968319, 10968320, 10968321, 10968322, 10968323, 10968324, 10968325, 10968326, 10968327, 10968328, 10968329, 10968330, 10968331, 10968332, 10968333, 10968334, 10968340, 10968341, 10968342, 10968343, 10968344, 10968345, 10968346, 10968347, 10968348, 10968349, 10968350, 10968351, 10968352, 10968353, 10968354, 10968355, 10968356, 10968357, 10968371, 10968372, 10968373, 10968374, 10968375, 10968376, 10968377, 10968378, 10968379, 10968380, 10968381, 10968382, 10968383, 10968384, 10968385, 10968386, 10968387, 10968388, 10968389, 10968390, 10968398, 10968399, 10968400, 10968402, 10968403, 10968404, 10968407, 10968408, 10968409, 10968410, 10968411, 10968412, 10968413, 10968414, 10968415, 10968416, 10968417, 10968418, 10968419, 10968420, 10968422, 10968423, 10968424, 10968425, 10968426, 10968427, 10968428, 10968429, 10968430, 10968431, 10968432, 10968433, 10968434, 10968435, 10968436, 10968437, 10968438, 10968439, 10968440, 10968441, 10968442, 10968443, 10968444, 10968445, 10968446, 10968447, 10968448, 10968449, 10968450, 10968451, 10968452, 10968453, 10968457, 10968458, 10968459, 10968460, 10968461, 10968462, 10968463, 10968464, 10968465, 10968466, 10968467, 10968468, 10968469, 10968470, 10968471, 10968472, 10968518, 10968519, 10968520, 10968521, 10968522, 10968523, 10968524, 10968527, 10968528, 10968529, 10968530, 10968534, 10968535, 10968540, 10968541, 10968542, 10968543, 10968544, 10968545, 10968546, 10968547, 10968548, 10968549, 10968550, 10968551, 10968552, 10968553, 10968562, 10968563, 10968564, 10968565, 10968566, 10968567, 10968568, 10968569, 10968570, 10968571, 10968666, 10968667, 10968668, 10968669, 10968670, 10968671, 10968674, 10968675, 10968676, 10968677, 10968687, 10968688, 10968689, 10968690, 10968691, 10968692, 10968693, 10968694, 10968695, 10968696, 10968697, 10968698, 10968699, 10968700, 10968701, 10968702, 10968703, 10968704, 10968705, 10968706, 10968713, 10968714, 10968715, 10968716, 10968717, 10968718, 10968719, 10968720, 10968730, 10968731, 10968732, 10968733, 10968734, 10968735, 10968740, 10968741, 10968742, 10968743, 10968744, 10968745, 10968746, 10968747, 10968748, 10968749, 10968750, 10968751, 10968752, 10968753, 10968754, 10968755, 10968801, 10968802, 10968803, 10968804, 10968805, 10968806, 10968807, 10968808, 10968809, 10968810, 10968811, 10968812, 10968815, 10968816, 10968817, 10968818, 10968819, 10968820, 10968821, 10968822, 10968823, 10968824, 10968825, 10968826, 10968827, 10968864, 10968865, 10968866, 10968867, 10968868, 10969463, 10969467, 10969472, 10969473, 10969474, 10969475, 10969480, 10969481, 10969485, 10969486, 10969491, 10969492, 10969533, 10969535, 10969540, 10969549, 10969550, 10969552, 10969575, 10969590, 10969592, 10969593, 10969594, 10969606, 10969608, 10969647, 10969648, 10969649, 10969657, 10969701, 10974001 },
                    Inactive = { 10849287, 720575940390128647, 10852480, 1008806316541843584, 1080863910579771520, 10853545, 1008806316541844649, 1080863910579772585, 10853552, 1008806316541844656, 1080863910579772592, 10853577, 1008806316541844681, 1080863910579772617, 10853578, 1008806316541844682, 1080863910579772618, 10853580, 1008806316541844684, 1080863910579772620, 10853581, 1008806316541844685, 1080863910579772621, 10853583, 1008806316541844687, 1080863910579772623, 10853584, 1008806316541844688, 1080863910579772624, 10857168, 72057594048785104, 1152921504617704144, 1369094286731487952, 10857169, 72057594048785105, 1152921504617704145, 1369094286731487953, 10857170, 72057594048785106, 1152921504617704146, 1369094286731487954, 10857171, 72057594048785107, 1152921504617704147, 1369094286731487955, 10857172, 72057594048785108, 1152921504617704148, 1369094286731487956, 10857173, 72057594048785109, 1152921504617704149, 1369094286731487957, 10857174, 288230376162568918, 360287970200496854, 10857175, 288230376162568919, 360287970200496855, 10857176, 288230376162568920, 360287970200496856, 10857177, 288230376162568921, 360287970200496857, 10857178, 288230376162568922, 360287970200496858, 10857179, 288230376162568923, 360287970200496859, 10857408, 72057594048785344, 1152921504617704384, 1369094286731488192, 10857475, 288230376162569219, 360287970200497155, 10907603, 504403158276403155, 506654958090088403, 504684633253113811, 507217908043509715, 10859635, 10859636, 10859637, 10871000, 10859638, 10859639, 10859640, 10859641, 10859642, 10859643, 10859644, 10859645, 10859646, 10859647, 10859648, 10859649, 10859650, 10859651, 10859652, 10859653, 10859654, 10859655, 10859656, 10859657, 10859658, 10859659, 10859660, 10859661, 10859662, 10859663, 10859664, 10859665, 10859666, 10859667, 10859668, 10859669, 10859670, 10859671, 10859672, 10859673, 10859674, 10859675, 10859677, 10859678, 10859679, 10859680, 10859681, 10859682, 10859683, 10859684, 10859685, 10859686, 10859687, 10859688, 10859689, 10859690, 10859691, 10859692, 10859693, 10859694, 10859695, 10859696, 10859697, 10859698, 10859699, 10859700, 10859701, 10859702, 10859703, 10859704, 10859705, 10859706, 10859707, 10859708, 10859709, 10859710, 10859711, 10859712, 10859713, 10859714, 10859715, 10859716, 10859717, 10859718, 10859719, 10859720, 10859721, 10859722, 10859723, 10859724, 10859725, 10859726, 10859727, 10859728, 10859729, 10859730, 10859731, 10859732, 10859733, 10859734, 10859735, 10859736, 10859737, 10859738, 10859739, 10859740, 10859741, 10859742, 10859743, 10859744, 10859745, 10859746, 10859747, 10859748, 10859749, 10859750, 10859751, 10859752, 10859753, 10859754, 10859755, 10859756, 10859757, 10859758, 10859759, 10859760, 10859761, 10859762, 10859763, 10859764, 10859765, 10859766, 10859767, 10859768, 10859769, 10859770, 10859771, 10859772, 10859773, 10859774, 10859775, 10859776, 10859777, 10859778, 10859779, 10859780, 10859781, 10859782, 10859783, 10859784, 10859785, 10859786, 10859787, 10859788, 10859789, 10859790, 10859791, 10859792, 10859793, 10859794, 10859795, 10859796, 10859797, 10859798, 10859799, 10859800, 10859801, 10859802, 10859803, 10859804, 10859805, 10859806, 10859807, 10859808, 10859809, 10859810, 10859811, 10859812, 10859813, 10859814, 10859815, 10859816, 10859817, 10859818, 10859819, 10859820, 10859821, 10859822, 10859823, 10859824, 10859825, 10859826, 10859827, 10859828, 10859829, 10859830, 10859831, 10859832, 10859833, 10859834, 10859835, 10859836, 10859837, 10859838, 10859839, 10859840, 10859841, 10859842, 10859843, 10859844, 10859845, 10859846, 10859847, 10859848, 10859849, 10859850, 10859851, 10859852, 10859853, 10859854, 10859855, 10859856, 10859857, 10859858, 10859859, 10859860, 10859861, 10859862, 10859863, 10859864, 10859865, 10859866, 10859867, 10859868, 10859869, 10859870, 10859871, 10859872, 10859873, 10859874, 10859875, 10859876, 10859877, 10859878, 10859879, 10859880, 10859881, 10859882, 10859883, 10859884, 10859885, 10859886, 10859887, 10859888, 10859889, 10859890, 10859891, 10859892, 10859893, 10859894, 10859895, 10859896, 10859897, 10859898, 10859899, 10859900, 10859901, 10859902, 10859903, 10859904, 10859905, 10859906, 10859907, 10859908, 10859909, 10859910, 10859911, 10859912, 10859913, 10859914, 10859915, 10859916, 10859917, 10859918, 10859919, 10859920, 10859921, 10859922, 10859923, 10859924, 10859925, 10859926, 10859927, 10859928, 10859929, 10859930, 10859931, 10859932, 10859933, 10859934, 10859935, 10859936, 10859937, 10859938, 10859939, 10859940, 10859941, 10859942, 10859943, 10859944, 10859945, 10859946, 10859947, 10859948, 10859949, 10859950, 10859951, 10859952, 10859953, 10859954, 10859955, 10859956, 10859957, 10859958, 10859959, 10859960, 10859961, 10859962, 10859963, 10859964, 10859965, 10859966, 10859967, 10859968, 10859969, 10859970, 10859971, 10859972, 10859973, 10859974, 10859975, 10859976, 10859977, 10859978, 10859979, 10859980, 10859981, 10859982, 10859983, 10859984, 10859985, 10859986, 10859987, 10859988, 10859989, 10859990, 10859991, 10859992, 10859993, 10859994, 10859995, 10859996, 10859997, 10859998, 10859999, 10860000, 10860001, 10860002, 10860003, 10860004, 10860005, 10860006, 10860007, 10860008, 10860009, 10860010, 10860011, 10860012, 10860013, 10860014, 10860015, 10860016, 10860017, 10860018, 10860019, 10860020, 10860021, 10860022, 10860023, 10860024, 10860025, 10860026, 10860027, 10860028, 10860029, 10860030, 10860031, 10860032, 10860033, 10860034, 10860035, 10860036, 10860037, 10860038, 10860039, 10860040, 10860041, 10860042, 10860043, 10860044, 10860045, 10860046, 10860047, 10860048, 10860049, 10860050, 10860051, 10860052, 10860053, 10860054, 10860055, 10860056, 10860057, 10860058, 10860059, 10860060, 10860061, 10860062, 10860063, 10860064, 10860065, 10860066, 10860067, 10860068, 10860069, 10860070, 10860071, 10860072, 10860073, 10860074, 10860075, 10860076, 10860077, 10860078, 10860079, 10860080, 10860081, 10860082, 10860083, 10860084, 10860085, 10860086, 10860087, 10860088, 10860089, 10860090, 10860091, 10860092, 10860093, 10860094, 10860095, 10860096, 10860097, 10860098, 10860099, 10860100, 10860101, 10860102, 10860103, 10860104, 10860105, 10860106, 10860107, 10860108, 10860109, 10860110, 10860111, 10860112, 10860113, 10860114, 10860115, 10860116, 10860117, 10860118, 10860119, 10860120, 10860121, 10860122, 10860123, 10860124, 10860125, 10860126, 10860127, 10860128, 10860129, 10860130, 10860131, 10860132, 10860133, 10860134, 10860135, 10860136, 10860137, 10860138, 10860139, 10860140, 10860141, 10860142, 10860143, 10860144, 10860145, 10860146, 10860147, 10860148, 10860149, 10860150, 10860151, 10860152, 10860153, 10860154, 10860155, 10860156, 10860157, 10860158, 10860159, 10860160, 10860161, 10860162, 10860163, 10860164, 10860165, 10860166, 10860167, 10860168, 10860169, 10860170, 10860171, 10860172, 10860173, 10860175, 10860176, 10860177, 10860178, 10860179, 10860180, 10860181, 10860182, 10860183, 10860184, 10860185, 10860186, 10860187, 10860188, 10860189, 10860190, 10860191, 10860192, 10860193, 10860194, 10860195, 10860196, 10860197, 10860198, 10860200, 10860201, 10860202, 10860203, 10860204, 10860205, 10860206, 10860207, 10860208, 10860209, 10860210, 10860211, 10860212, 10860213, 10860214, 10860215, 10860216, 10860217, 10860218, 10860219, 10860220, 10860221, 10860222, 10860223, 10860224, 10860225, 10860226, 10860227, 10860228, 10860229, 10860230, 10860231, 10860232, 10860233, 10860234, 10860235, 10860236, 10860237, 10860238, 10860239, 10860240, 10860241, 10860242, 10860243, 10860244, 10860245, 10860246, 10860247, 10860248, 10860249, 10860250, 10860251, 10860252, 10860253, 10860254, 10860255, 10860256, 10860257, 10860258, 10860259, 10860260, 10860261, 10860262, 10860263, 10860264, 10860265, 10860266, 10860267, 10860268, 10860269, 10860270, 10860271, 10860272, 10860273, 10860274, 10860275, 10860276, 10860277, 10860278, 10860279, 10860280, 10860281, 10860282, 10860283, 10860284, 10860285, 10860286, 10860287, 10860288, 10860289, 10860290, 10860291, 10860292, 10860293, 10860294, 10860295, 10860296, 10860297, 10860298, 10860299, 10860300, 10860301, 10860302, 10860303, 10860304, 10860305, 10860306, 10860307, 10860308, 10860309, 10860310, 10860311, 10860312, 10860313, 10860314, 10860315, 10860316, 10860317, 10860318, 10860319, 10860320, 10860321, 10860322, 10860323, 10860324, 10860325, 10860326, 10860327, 10860328, 10860329, 10860330, 10860331, 10860332, 10860333, 10860334, 10860335, 10860336, 10860337, 10860338, 10860339, 10860340, 10860341, 10860342, 10860343, 10860344, 10860345, 10860346, 10860347, 10860348, 10860349, 10860350, 10860351, 10860352, 10860353, 10860354, 10860355, 10860356, 10860357, 10860358, 10860359, 10860360, 10860361, 10860362, 10860363, 10860365, 10860366, 10860367, 10860368, 10860369, 10860370, 10860371, 10860372, 10860373, 10860374, 10860375, 10860376, 10860377, 10860378, 10860379, 10860380, 10860381, 10860382, 10860383, 10860384, 10860385, 10860386, 10860387, 10860388, 10860389, 10860390, 10860391, 10860392, 10860393, 10860394, 10860395, 10860396, 10860397, 10860398, 10860399, 10860400, 10860401, 10860402, 10860403, 10860404, 10860405, 10860406, 10860407, 10860408, 10860409, 10860410, 10860411, 10860412, 10860413, 10860414, 10860415, 10860416, 10860417, 10860418, 10860419, 10860420, 10860421, 10860422, 10860423, 10860424, 10860425, 10860426, 10860427, 10860428, 10860429, 10860430, 10860431, 10860432, 10860433, 10860434, 10860435, 10860436, 10860437, 10860438, 10860439, 10860440, 10860441, 10860442, 10860443, 10860444, 10860445, 10860446, 10860447, 10860448, 10860449, 10860450, 10860517, 10860543, 72057594048788479, 10860544, 72057594048788480, 10860545, 10860547, 10860465, 10860466, 10860467, 10860468, 10860469, 10860470, 10860471, 10860472, 10860473, 10860474, 10860475, 10860476, 10860477, 10860478, 10860500, 10861464, 10861465, 10861466, 10861467, 10861468, 10861469, 10861470, 10861471, 10861472, 10861473, 10861474, 10861475, 10861476, 10861477, 10861481, 10861483, 10861484, 10861485, 10861486, 10861487, 10861488, 10861489, 10861490, 10861491, 10861492, 10861493, 10861494, 10861495, 10861496, 10861499, 10870696, 10870697, 10870698, 10862184, 1729382256921132648, 72057594048790120, 144115188086718056, 216172782124645992, 288230376162573928, 432345564238429800, 504403158276357736, 576460752314285672, 1585267068845276776, 1657324662883204712, 1801439850959060584, 10862185, 1729382256921132649, 72057594048790121, 144115188086718057, 216172782124645993, 288230376162573929, 432345564238429801, 504403158276357737, 576460752314285673, 1585267068845276777, 1657324662883204713, 1801439850959060585, 10862186, 1729382256921132650, 72057594048790122, 144115188086718058, 216172782124645994, 288230376162573930, 432345564238429802, 504403158276357738, 576460752314285674, 1585267068845276778, 1657324662883204714, 1801439850959060586, 10862187, 1729382256921132651, 72057594048790123, 144115188086718059, 216172782124645995, 288230376162573931, 432345564238429803, 504403158276357739, 576460752314285675, 1585267068845276779, 1657324662883204715, 1801439850959060587, 10862188, 1729382256921132652, 72057594048790124, 144115188086718060, 216172782124645996, 288230376162573932, 432345564238429804, 504403158276357740, 576460752314285676, 1585267068845276780, 1657324662883204716, 1801439850959060588, 10862189, 1729382256921132653, 72057594048790125, 144115188086718061, 216172782124645997, 288230376162573933, 432345564238429805, 504403158276357741, 576460752314285677, 1585267068845276781, 1657324662883204717, 1801439850959060589, 10862190, 1729382256921132654, 72057594048790126, 144115188086718062, 216172782124645998, 288230376162573934, 432345564238429806, 504403158276357742, 576460752314285678, 1585267068845276782, 1657324662883204718, 1801439850959060590, 10862191, 1729382256921132655, 72057594048790127, 144115188086718063, 216172782124645999, 288230376162573935, 432345564238429807, 504403158276357743, 576460752314285679, 1585267068845276783, 1657324662883204719, 1801439850959060591, 10866536, 72057594048794472, 144115188086722408, 720575940390145896, 10865420, 1297036692693568268, 864691128466000652, 865254078419421964, 865268372070583052, 865278267675233036, 865280466698488588, 865269471582210828, 865270571093838604, 865271670605466380, 865273869628721932, 865274969140349708, 865277168163605260, 865276068651977484, 865272770117094156, 865279367186860812, 865535553396132620, 865554245093804812, 865560842163571468, 865549847047293708, 865550946558921484, 865552046070549260, 865553145582177036, 865555344605432588, 865556444117060364, 865557543628688140, 865558643140315916, 865559742651943692, 865561941675199244, 864972603442711308, 865817028372843276, 865818127884471052, 866098503349553932, 866099602861181708, 866379978326264588, 866381077837892364, 866661453302975244, 866662552814603020, 866942928279685900, 866944027791313676, 867224403256396556, 867225502768024332, 867505878233107212, 867508077256362764, 867787353209817868, 867789552233073420, 868068828186528524, 868071027209784076, 868350303163239180, 868352502186494732, 868631778139949836, 868633977163205388, 868913253116660492, 868915452139916044, 72057594048793356, 144115188086721292, 216172782124649228, 288230376162577164, 1513209474807352076, 1585267068845280012, 1729382256921135884, 1801439850959063820, 1873497444996991756, 11156915, 72057594049084851, 144115188087012787, 10866554, 2810246167490056058, 2882303761527983994, 2954361355565911930, 216172782124650362, 216454257101361018, 216477346845544314, 216478446357172090, 216455356612988794, 216457555636244346, 216458655147872122, 216479545868799866, 216459754659499898, 216459758954467194, 216460854171127674, 216460858466094970, 216461953682755450, 216461957977722746, 216463053194383226, 216463057489350522, 216464152706011002, 216464157000978298, 216465252217638778, 216465256512606074, 216466351729266554, 216466356024233850, 216467451240894330, 216467455535861626, 216468550752522106, 216468555047489402, 216469650264149882, 216469654559117178, 216470749775777658, 216470754070744954, 216471849287405434, 216471853582372730, 216472948799033210, 216472953094000506, 216474048310660986, 216474052605628282, 216475147822288762, 216475152117256058, 216476247333916538, 216476251628883834, 216480645380427642, 216480649675394938, 216487242450194298, 216488341961822074, 576460752314290042, 577868127197843322, 577869226709471098, 577869231004438394, 577870326221098874, 577870330516066170, 577871425732726650, 577871430027693946, 577872525244354426, 577872529539321722, 577873624755982202, 577873629050949498, 577874724267609978, 577874728562577274, 577875823779237754, 577875828074205050, 577876923290865530, 577876927585832826, 578149602174553978, 578150701686181754, 578150705981149050, 578151801197809530, 578151805492776826, 578152900709437306, 578152905004404602, 578154000221065082, 578154004516032378, 578155099732692858, 578155104027660154, 578156199244320634, 578156203539287930, 578157298755948410, 578157303050915706, 578158398267576186, 578158402562543482, 578431077151264634, 578432176662892410, 578432180957859706, 578433276174520186, 578433280469487482, 578434375686147962, 578434379981115258, 578435475197775738, 578435479492743034, 578436574709403514, 578436579004370810, 578437674221031290, 578437678515998586, 578438773732659066, 578438778027626362, 578439873244286842, 578439877539254138, 576742227291000698, 576743326802628474, 576743331097595770, 576744426314256250, 576744430609223546, 576745525825884026, 576745530120851322, 576746625337511802, 576746629632479098, 576747724849139578, 576747729144106874, 576748824360767354, 576748828655734650, 576749923872395130, 576749928167362426, 576751023384022906, 576751027678990202, 577023702267711354, 577024801779339130, 577024806074306426, 577025901290966906, 577025905585934202, 577027000802594682, 577027005097561978, 577028100314222458, 577028104609189754, 577029199825850234, 577029204120817530, 577030299337478010, 577030303632445306, 577031398849105786, 577031403144073082, 577032498360733562, 577032502655700858, 577305177244422010, 577306276756049786, 577306281051017082, 577307376267677562, 577307380562644858, 577308475779305338, 577308480074272634, 577309575290933114, 577309579585900410, 577310674802560890, 577310679097528186, 577311774314188666, 577311778609155962, 577312873825816442, 577312878120783738, 577313973337444218, 577313977632411514, 578994027104685946, 578995126616313722, 578995130911281018, 578995135206248314, 578995139501215610, 578995143796182906, 579275502081396602, 579277701104652154, 579279900127907706, 579278800616279930, 579280999639535482, 579282099151163258, 579283198662791034, 579284298174418810, 579285397686046586, 579556977058107258, 579558076569735034, 579560275592990586, 288230376162578298, 294141350673502074, 294422825650212730, 288511851139288954, 289074801092710266, 289356276069420922, 294704300626923386, 289637751046131578, 289638850557759354, 289919226022842234, 289920325534470010, 290200700999552890, 290201800511180666, 290482175976263546, 290483275487891322, 290763650952974202, 290764750464601978, 291045125929684858, 291046225441312634, 291326600906395514, 291327700418023290, 291608075883106170, 291609175394733946, 291889550859816826, 291890650371444602, 292171025836527482, 292172125348155258, 292452500813238138, 292453600324865914, 292733975789948794, 292735075301576570, 293015450766659450, 293016550278287226, 293296925743370106, 293298025254997882, 293578400720080762, 293579500231708538, 293859875696791418, 293860975208419194, 294985775603634042, 294986875115261818, 296111675510476666, 296393150487187322, 648518346352217978, 648799821328928634, 648800920840556410, 648800925135523706, 648800929430491002, 648800933725458298, 648800938020425594, 649644246259060602, 649645345770688378, 649645350065655674, 649646445282316154, 649646449577283450, 649647544793943930, 649647549088911226, 649648644305571706, 649648648600539002, 649649743817199482, 649649748112166778, 649650843328827258, 649650847623794554, 649651942840455034, 649651947135422330, 649653042352082810, 649653046647050106, 649654141863710586, 649654146158677882, 649655241375338362, 649655245670305658, 649656340886966138, 649656345181933434, 649657440398593914, 649657444693561210, 649658539910221690, 649658544205188986, 649659639421849466, 649659643716816762, 649660738933477242, 649660743228444538, 649925721235771258, 649926820747399034, 649926825042366330, 649927920259026810, 649927924553994106, 649929019770654586, 649929024065621882, 649930119282282362, 649930123577249658, 649931218793910138, 649931223088877434, 649932318305537914, 649932322600505210, 649933417817165690, 649933422112132986, 649934517328793466, 649934521623760762, 649935616840421242, 649935621135388538, 649936716352049018, 649936720647016314, 649937815863676794, 649937820158644090, 649938915375304570, 649938919670271866, 649940014886932346, 649940019181899642, 649941114398560122, 649941118693527418, 649942213910187898, 649942218205155194, 650207196212481914, 650208295724109690, 650208300019076986, 650209395235737466, 650209399530704762, 650210494747365242, 650210499042332538, 650211594258993018, 650211598553960314, 650212693770620794, 650212698065588090, 650213793282248570, 650213797577215866, 650214892793876346, 650214897088843642, 650215992305504122, 650215996600471418, 650217091817131898, 650217096112099194, 650218191328759674, 650218195623726970, 650219290840387450, 650219295135354746, 650220390352015226, 650220394646982522, 650221489863643002, 650221494158610298, 650222589375270778, 650222593670238074, 650223688886898554, 650223693181865850, 649081296305639290, 649083495328894842, 649085694352150394, 649084594840522618, 649086793863778170, 649087893375405946, 649088992887033722, 649090092398661498, 649091191910289274, 649362771282349946, 649363870793977722, 2017612633072848762, 2024368032513904506, 2025212457444036474, 2026056882374168442, 2025775407397457786, 2025493932420747130, 2025531315816091514, 2025532415327719290, 2025533514839347066, 2025534614350974842, 2025535713862602618, 2025536813374230394, 2025537912885858170, 2025539012397485946, 2025540111909113722, 2025541211420741498, 2025542310932369274, 2025543410443997050, 2025544509955624826, 2025545609467252602, 2025546708978880378, 2025547808490508154, 2029434582094696314, 2029435681606324090, 2029716057071406970, 2029717156583034746, 2030279007024828282, 2030280106536456058, 2030841956978249594, 2030843056489877370, 2029997532048117626, 2029998631559745402, 2030560482001538938, 2030561581513166714, 2161727821148704634, 2168483220589760378, 2169327645519892346, 2170172070450024314, 2169890595473313658, 2169609120496603002, 2169646503891947386, 2169647603403575162, 2169648702915202938, 2169649802426830714, 2169650901938458490, 2169652001450086266, 2169653100961714042, 2169654200473341818, 2169655299984969594, 2169656399496597370, 2169657499008225146, 2169658598519852922, 2169659698031480698, 2169660797543108474, 2169661897054736250, 2169662996566364026, 2173549770170552186, 2173550869682179962, 2173831245147262842, 2173832344658890618, 2174394195100684154, 2174395294612311930, 2174957145054105466, 2174958244565733242, 2174112720123973498, 2174113819635601274, 2174675670077394810, 2174676769589022586, 3746994889983119226, 3819052484021047162, 4395513236324470650, 4467570830362398586, 10870860, 2810246167490060364, 2882303761527988300, 2954361355565916236, 3602879701907267660, 3606257401627795532, 3606259600651051084, 3606259604946018380, 3606259609240985676, 3606259613535952972, 3606259617830920268, 3606259622125887564, 3606259626420854860, 3606259630715822156, 3606259635010789452, 3606259639305756748, 5044031582665826380, 5044313057642537036, 5044314157154164812, 5044315256665792588, 5044316356177420364, 5044317455689048140, 5044318555200675916, 5044319654712303692, 5044594532619247692, 5044595632130875468, 5044596731642503244, 5044597831154131020, 5044598930665758796, 5044600030177386572, 5044601129689014348, 5047690757363064908, 5047696254921203788, 5050786982106882124, 5116089176703754316, 5116370651680464972, 5116371751192092748, 5116372850703720524, 5116373950215348300, 5116375049726976076, 5116376149238603852, 5116377248750231628, 5116652126657175628, 5116653226168803404, 5116654325680431180, 5116655425192058956, 5116656524703686732, 5116657624215314508, 5116658723726942284, 5119748351400992844, 10895951, 1513209474807382607, 1585267068845310543, 72057594048823887, 144115188086751823, 216172782124679759, 288230376162607695, 1657324662883238479, 360287970200535631, 360569445177246287, 432345564238463567, 432627039215174223, 504403158276391503, 504684633253102159, 576460752314319439, 576742227291030095, 648518346352247375, 648799821328958031, 720575940390175311, 720857415366885967, 792633534428103247, 792915009404813903, 864691128466031183, 864972603442741839, 936748722503959119, 937030197480669775, 1008806316541887055, 1009087791518597711, 1080863910579814991, 1081145385556525647, 1152921504617742927, 1153202979594453583, 1224979098655670863, 1225260573632381519, 1297036692693598799, 1297318167670309455, 1369094286731526735, 1369375761708237391, 1441151880769454671, 1441433355746165327, 1729382256921166415, 1729663731897877071, 2161727821148734031, 2233785415186661967, 10907764, 1729382256921178228, 1945555039034962036, 2161727821148745844, 2089670227110817908, 2017612633072889972, 2027182782281052276, 2027464257257762932, 2027745732234473588, 2028027207211184244, 2028308682187894900, 2028590157164605556, 2028871632141316212, 2029153107118026868, 2029434582094737524, 2029716057071448180, 2029997532048158836, 2030279007024869492, 2030560482001580148, 2030841956978290804, 2031123431955001460, 2031404906931712116, 3026418949603881076, 3026700424580591732, 3098476543641809012, 3098758018618519668, 3242591731717664884, 3242873206694375540, 3386706919793520756, 3386988394770231412, 3170534137679736948, 3170815612656447604, 3314649325755592820, 3314930800732303476, 10907770, 72057594048835706, 72339069025546362, 72346765606940794, 72347865118568570, 72348964630196346, 72350064141824122, 72354462188335226, 72355561699963002, 72356661211590778, 72359959746474106, 72369855351124090, 72370954862751866, 72372054374379642, 72373153886007418, 72375352909262970, 72376452420890746, 72377551932518522, 72378651444146298, 144115188086763642, 144396663063474298, 144404359644868730, 144405459156496506, 144406558668124282, 144407658179752058, 144412056226263162, 144413155737890938, 144414255249518714, 144417553784402042, 144427449389052026, 144428548900679802, 144429648412307578, 144430747923935354, 144432946947190906, 144434046458818682, 144435145970446458, 144436245482074234, 216172782124691578, 216454257101402234, 216461953682796666, 216463053194424442, 216464152706052218, 216465252217679994, 216469650264191098, 216470749775818874, 216471849287446650, 216475147822329978, 216485043426979962, 216486142938607738, 216487242450235514, 216488341961863290, 216490540985118842, 216491640496746618, 216492740008374394, 216493839520002170, 288230376162619514, 288511851139330170, 360287970200547450, 360569445177258106, 432345564238475386, 432627039215186042, 504403158276403322, 504684633253113978, 576460752314331258, 576742227291041914, 648518346352259194, 648799821328969850, 720575940390187130, 720857415366897786, 792633534428115066, 792915009404825722, 10907771, 72057594048835707, 72339069025546363, 144115188086763643, 144396663063474299, 216172782124691579, 216454257101402235, 288230376162619515, 288511851139330171, 360287970200547451, 360569445177258107, 432345564238475387, 432627039215186043, 504403158276403323, 504684633253113979, 576460752314331259, 576742227291041915, 648518346352259195, 648799821328969851, 720575940390187131, 720857415366897787, 792633534428115067, 792915009404825723, 2017612633072889979, 10913860, 144115188086769732, 144396663063480388, 288230376162625604, 288511851139336260, 360287970200553540, 360569445177264196, 432345564238481476, 432627039215192132, 10869502, 1729382256921139966, 72057594048797438, 144115188086725374, 216172782124653310, 288230376162581246, 432345564238437118, 504403158276365054, 576460752314292990, 1585267068845284094, 1657324662883212030, 1801439850959067902, 10869503, 1729382256921139967, 72057594048797439, 144115188086725375, 216172782124653311, 288230376162581247, 432345564238437119, 504403158276365055, 576460752314292991, 1585267068845284095, 1657324662883212031, 1801439850959067903, 10869504, 1729382256921139968, 72057594048797440, 144115188086725376, 216172782124653312, 288230376162581248, 432345564238437120, 504403158276365056, 576460752314292992, 1585267068845284096, 1657324662883212032, 1801439850959067904, 10869505, 1729382256921139969, 72057594048797441, 144115188086725377, 216172782124653313, 288230376162581249, 432345564238437121, 504403158276365057, 576460752314292993, 1585267068845284097, 1657324662883212033, 1801439850959067905, 10869506, 1729382256921139970, 72057594048797442, 144115188086725378, 216172782124653314, 288230376162581250, 432345564238437122, 504403158276365058, 576460752314292994, 1585267068845284098, 1657324662883212034, 1801439850959067906, 10869507, 1729382256921139971, 72057594048797443, 144115188086725379, 216172782124653315, 288230376162581251, 432345564238437123, 504403158276365059, 576460752314292995, 1585267068845284099, 1657324662883212035, 1801439850959067907, 10870688, 72057594048798624, 1152921504617717664, 1369094286731501472, 10870689, 72057594048798625, 1152921504617717665, 1369094286731501473, 10870690, 288230376162582434, 360287970200510370, 10870691, 288230376162582435, 360287970200510371, 10885836, 72057594048813772, 504403158276381388, 10885837, 72057594048813773, 360287970200525517, 504403158276381389, 504684633253092045, 10885842, 72057594048813778, 360287970200525522, 504403158276381394, 504684633253092050, 10885843, 72057594048813779, 360287970200525523, 504403158276381395, 504684633253092051, 10885844, 72057594048813780, 360287970200525524, 504403158276381396, 504684633253092052, 10885846, 72057594048813782, 144115188086741718, 1297036692693588694, 1297881117623720662, 1301821767297669846, 1801439850959084246, 2161727821148723926, 2233785415186651862, 2305843009224579798, 2017612633072868054, 2089670227110795990, 10885847, 72057594048813783, 144115188086741719, 1297036692693588695, 1297881117623720663, 1301821767297669847, 1801439850959084247, 2161727821148723927, 2233785415186651863, 2305843009224579799, 2017612633072868055, 2089670227110795991, 10885848, 72057594048813784, 144115188086741720, 1297036692693588696, 1297881117623720664, 1301821767297669848, 1801439850959084248, 2161727821148723928, 2233785415186651864, 2305843009224579800, 2017612633072868056, 2089670227110795992, 10885849, 72057594048813785, 144115188086741721, 1297036692693588697, 1297881117623720665, 1301821767297669849, 1801439850959084249, 2161727821148723929, 2233785415186651865, 2305843009224579801, 2017612633072868057, 2089670227110795993, 10885850, 72057594048813786, 360287970200525530, 504403158276381402, 504684633253092058, 10885851, 72057594048813787, 360287970200525531, 504403158276381403, 504684633253092059, 10885852, 72057594048813788, 360287970200525532, 504403158276381404, 504684633253092060, 10885853, 72057594048813789, 360287970200525533, 504403158276381405, 504684633253092061, 10885854, 72057594048813790, 144115188086741726, 1297036692693588702, 1297881117623720670, 1301821767297669854, 1801439850959084254, 2161727821148723934, 2233785415186651870, 2305843009224579806, 2017612633072868062, 2089670227110795998, 10885855, 72057594048813791, 144115188086741727, 1297036692693588703, 1297881117623720671, 1301821767297669855, 1801439850959084255, 2161727821148723935, 2233785415186651871, 2305843009224579807, 2017612633072868063, 2089670227110795999, 10885856, 72057594048813792, 144115188086741728, 1297036692693588704, 1297881117623720672, 1301821767297669856, 1801439850959084256, 2161727821148723936, 2233785415186651872, 2305843009224579808, 2017612633072868064, 2089670227110796000, 10885857, 72057594048813793, 144115188086741729, 1297036692693588705, 1297881117623720673, 1301821767297669857, 1801439850959084257, 2161727821148723937, 2233785415186651873, 2305843009224579809, 2017612633072868065, 2089670227110796001, 11159679, 72057594049087615, 360287970200799359, 504403158276655231, 504684633253365887, 11159680, 72057594049087616, 360287970200799360, 504403158276655232, 504684633253365888, 11159681, 72057594049087617, 360287970200799361, 504403158276655233, 504684633253365889, 11159684, 72057594049087620, 144115188087015556, 1297036692693862532, 1297881117623994500, 1301821767297943684, 1801439850959358084, 2161727821148997764, 2233785415186925700, 2305843009224853636, 2017612633073141892, 2089670227111069828, 11159685, 72057594049087621, 144115188087015557, 1297036692693862533, 1297881117623994501, 1301821767297943685, 1801439850959358085, 2161727821148997765, 2233785415186925701, 2305843009224853637, 2017612633073141893, 2089670227111069829, 11159686, 72057594049087622, 144115188087015558, 1297036692693862534, 1297881117623994502, 1301821767297943686, 1801439850959358086, 2161727821148997766, 2233785415186925702, 2305843009224853638, 2017612633073141894, 2089670227111069830, 10883630, 72057594048811566, 10883663, 10883664, 10913168, 10913176 },
                    Npcs =
                    [
                        new()
                        {
                            ENpcId = 1022686,
                            Position = new(95.60f, 0.00f, 90.00f),
                            Rotation = 0.60f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(105.20f, 0.00f, 89.80f),
                            Rotation = -0.68f,
                            Scale = 1.14f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(105.60f, 0.00f, 90.60f),
                            Rotation = -2.18f,
                            Scale = 0.93f
                        },
                        new()
                        {
                            ENpcId = 1025939,
                            Position = new(104.40f, 0.00f, 89.80f),
                            Rotation = 1.57f,
                            Scale = 1.07f
                        }
                    ]
                }
            });
        }

        private void AddPreset(PresetModel preset)
        {
            try
            {
                Validate(preset);
            }
            catch (Exception e)
            {
                Services.Log.Warning(e, $"Failed to add base preset '{preset.FileName}'");
                return;
            }

            presets.Add(preset.FileName, preset);
        }
    }
}