using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MapAPI.Models
{
    /* Location as a datacontract */
    [DataContract(Namespace = "", Name = "Location")]
    public class LocationContract
    {
        /* Default constructor */
        public LocationContract()
        {
            this.LocationId = -1;
            this.SwedishMainName = "";
            this.EnglishMainName = "";
            this.Names = new List<Name>();
            this.AreaId = -1;
            this.LocationTypeId = -1;
            this.FloorNumber = -1;
            this.Latitude = Decimal.Parse("1");
            this.Longitude = Decimal.Parse("1");
        }

        /* Constructor with location */
        public LocationContract(Location l)
        {
            this.LocationId = l.location_id;
            this.AreaId = l.area_id;
            this.LocationTypeId = l.location_type_id;
            this.FloorNumber = (l.floor_nr != null) ? (int)(l.floor_nr) : 0;
            this.Latitude = l.latitude;
            this.Longitude = l.longitude;
            this.Names = new List<Name>();

            foreach (LocationName locationName in l.LocationNames)
            {
                Name name = new Name();
                name.Swedish = locationName.location_name_swe;
                
                if (locationName.is_main)
                    this.SwedishMainName = locationName.location_name_swe;

                if (locationName.location_name_swe != locationName.location_name_eng)
                {
                    name.English = locationName.location_name_eng;

                    if (locationName.is_main)
                        this.EnglishMainName = locationName.location_name_eng;
                }
                else
                {
                    name.English = "";

                    if (locationName.is_main)
                        this.EnglishMainName = "";
                }

                Names.Add(name);
            }
        }

        /* Location to Location contract */
        public static LocationContract ToContract(Location l)
        {
            return new LocationContract(l);
        }

        /* Locaiton to Location contract */
        public static IEnumerable<LocationContract> ToContracts(IEnumerable<Location> locations)
        {
            List<LocationContract> lcs = new List<LocationContract>();

            foreach (Location location in locations)
                lcs.Add(LocationContract.ToContract(location));

            return lcs.AsEnumerable();
        }

        [DataMember(Name="Location_ID")]
        public int LocationId { get; set; }

        [DataMember(Name = "Location_Type_ID")]
        public short LocationTypeId { get; set; }

        [DataMember(Name = "Area_ID")]
        public int AreaId { get; set; }

        [DataMember(Name = "Latitude")]
        public decimal Latitude { get; set; }

        [DataMember(Name = "Longitude")]
        public decimal Longitude { get; set; }

        [DataMember(Name = "Floor_Number")]
        public int FloorNumber { get; set; }

        [DataMember(Name = "Names")]
        public List<Name> Names { get; set; }

        [DataMember(Name = "Swedish_Main_Name")]
        public string SwedishMainName { get; set; }

        [DataMember(Name = "English_Main_Name")]
        public string EnglishMainName { get; set; }
    }
}