using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MapAPI.Models
{
    /* LocationType as a datacontract */
    [DataContract(Namespace = "", Name = "LocationType")]
    public class LocationTypeContract
    {
        /* Default constructor */
        public LocationTypeContract()
        {
            this.LocationTypeId = -1;
            this.IconID = -1;
            this.Name = new Name();
            this.Name.Swedish = "";
            this.Name.English = "";
            this.IconUrl = "";
        }

        /* Constructor with locationtype */
        public LocationTypeContract(LocationType locationType)
        {
            this.LocationTypeId = locationType.location_type_id;
            this.IconID = (locationType.icon_id != null) ? (int)(locationType.icon_id) : 0;
            this.Name = new Name();
            this.Name.Swedish = locationType.location_type_swe;
            this.Name.English = locationType.location_type_eng;
            this.IconUrl = (locationType.Icon != null) ? locationType.Icon.icon_link : "";
        }

        /* LocationType to LocationType contract */
        public static LocationTypeContract ToContract(LocationType locationType)
        {
            return new LocationTypeContract(locationType);
        }

        /* LocationTypes to LocationTypes contract */
        public static IEnumerable<LocationTypeContract> ToContracts(IEnumerable<LocationType> locationTypes)
        {
            List<LocationTypeContract> ltcs = new List<LocationTypeContract>();

            foreach (LocationType locationType in locationTypes)
                ltcs.Add(LocationTypeContract.ToContract(locationType));

            return ltcs.AsEnumerable();
        }

        [DataMember(Name = "Location_Type_ID")]
        public int LocationTypeId { get; set; }

        [DataMember(Name = "Icon_ID")]
        public int IconID { get; set; }

        [DataMember(Name = "Name")]
        public Name Name { get; set; }

        [DataMember(Name = "Icon_URL")]
        public string IconUrl { get; set; }
    }
}