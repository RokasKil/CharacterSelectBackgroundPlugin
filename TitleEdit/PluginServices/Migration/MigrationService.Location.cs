using System;
using Newtonsoft.Json;
using System.Linq;
using TitleEdit.Data.Lobby;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.PluginServices.Migration
{
    public partial class MigrationService
    {
        public LocationModel MigrateLocation(string locationTextData) => MigrateLocation(locationTextData, out _);

        public LocationModel MigrateLocation(string locationTextData, out bool changed)
        {
            var location = JsonConvert.DeserializeObject<LocationModel>(locationTextData);
            changed = true;
            switch (location.Version)
            {
                case 1:
                    location = MigrateV1(location);
                    goto case 2;
                case 2:
                    location = MigrateV2(location);
                    goto case 3;
                case 3:
                    location = MigrateV3(location);
                    goto case 4;
                case 4:
                    location = MigrateV4(location);
                    goto case 5;
                case 5:
                    location = MigrateV5(location);
                    break;
                default:
                    changed = false;
                    break;
            }

            return location;
        }

        private LocationModel MigrateV1(LocationModel location)
        {
            Services.Log.Info($"Migrating location to v2");
            location.Version = 2;
            location.LocationType = LocationType.CharacterSelect;
            return location;
        }

        private LocationModel MigrateV2(LocationModel location)
        {
            Services.Log.Info($"Migrating location to v3");
            location.Version = 3;
            location.UiColor = UiColors.Dawntrail;
            return location;
        }

        private LocationModel MigrateV3(LocationModel location)
        {
            Services.Log.Info($"Migrating location to v4");
            location.Version = 4;
            location.Festivals ??= new Festival[LocationModel.OLD_FESTIVAL_COUNT];
            location.Active ??= [];
            location.Inactive ??= [];
            location.VfxTriggerIndexes ??= [];
            location.SaveFestivals = location.Festivals.Length == LocationModel.OLD_FESTIVAL_COUNT && location.Festivals.Any(festival => festival is not { Id: 0, Phase: 0 });
            location.SaveLayout = location.UseVfx = (location.Active.Count > 0 || location.Inactive.Count > 0);
            return location;
        }

        private LocationModel MigrateV4(LocationModel location)
        {
            Services.Log.Info($"Migrating location to v5");
            location.Version = 5;
            location.TitleScreenMovie = TitleScreenMovie.Unspecified;
            return location;
        }

        private LocationModel MigrateV5(LocationModel location)
        {
            Services.Log.Info($"Migrating location to v6");
            location.Version = 6;
            var newFestivals = new Festival[LocationModel.FESTIVAL_COUNT];
            if (location.Festivals != null)
            {
                Array.Copy(location.Festivals, newFestivals,
                           Math.Min(location.Festivals.Length, LocationModel.FESTIVAL_COUNT));
            }

            location.Festivals = newFestivals;
            return location;
        }
    }
}
