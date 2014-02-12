using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAPI.Models
{
    public class MobileLocation
    {
        public MobileLocation(Location location)
        {
            this.Location = LocationContract.ToContract(location);
            this.AreaName = new Name();
            this.LocationTypeName = new Name();

            if (location.Area != null)
            {
                this.AreaName.Swedish = location.Area.area_swe;
                this.AreaName.English = location.Area.area_eng;
            }
            else
            {
                this.AreaName.Swedish = "";
                this.AreaName.English = "";
            }

            if (location.LocationType != null)
            {
                this.LocationTypeName.Swedish = location.LocationType.location_type_swe;
                this.LocationTypeName.English = location.LocationType.location_type_eng;
            }
            else
            {
                this.LocationTypeName.Swedish = "";
                this.LocationTypeName.English = "";
            }

            this.CityName = (location.Area.City != null) ? location.Area.City.city1 : "";
        }

        public LocationContract Location { get; set; }
        public Name AreaName { get; set; }
        public Name LocationTypeName { get; set; }
        public string CityName { get; set; }

        public static MobileLocation ToMobile(Location location)
        {
            return new MobileLocation(location);
        }

        public static IEnumerable<MobileLocation> ToMobiles(IEnumerable<Location> locations)
        {
            List<MobileLocation> lms = new List<MobileLocation>();

            foreach (Location location in locations)
                lms.Add(MobileLocation.ToMobile(location));

            return lms.AsEnumerable();
        }
    }

    public class MobileArea
    {
        public MobileArea(Area area)
        {
            this.Area = AreaContract.ToContract(area);
            this.CityName = (area.City != null) ? area.City.city1 : "";
        }

        public static MobileArea ToMobile(Area area)
        {
            return new MobileArea(area);
        }

        public static IEnumerable<MobileArea> ToMobiles(IEnumerable<Area> areas)
        {
            List<MobileArea> ams = new List<MobileArea>();

            foreach (Area area in areas)
                ams.Add(MobileArea.ToMobile(area));

            return ams.AsEnumerable();
        }

        public AreaContract Area { get; set; }
        public string CityName { get; set; }
    }
}