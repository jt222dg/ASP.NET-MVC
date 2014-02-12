using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace MapAPI.Models
{
    /* Area as a datacontract */
    [DataContract(Namespace = "", Name = "Area")]
    public class AreaContract
    {
        /* Default constructor */
        public AreaContract()
        {
            this.AreaId = -1;
            this.CityId = -1;
            this.Name = new Name();
            this.Name.Swedish = "";
            this.Name.English = "";
            this.Latitude = Decimal.Parse("1");
            this.Longitude = Decimal.Parse("1");
        }

        /* Constructor with area */
        public AreaContract(Area a)
        {
            this.AreaId = a.area_id;
            this.CityId = a.city_id;
            this.Name = new Name();
            this.Name.Swedish = a.area_swe;
            this.Name.English = (a.area_swe != a.area_eng) ? a.area_eng : "";
            this.Latitude = a.latitude;
            this.Longitude = a.longitude;
        }

        /* Area to Area contract */
        public static AreaContract ToContract(Area a)
        {
            return new AreaContract(a);
        }

        /* Areas to Area contracts */
        public static IEnumerable<AreaContract> ToContracts(IEnumerable<Area> areas)
        {
            List<AreaContract> acs = new List<AreaContract>();

            foreach (Area area in areas)
                acs.Add(AreaContract.ToContract(area));

            return acs.AsEnumerable();
        }

        [DataMember(Name="ID")]
        public int AreaId { get; set; }

        [DataMember(Name="City_ID")]
        public int CityId { get; set; }

        [DataMember(Name = "Name")]
        public Name Name { get; set; }

        [DataMember(Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [DataMember(Name = "Longitude")]
        public decimal? Longitude { get; set; }
    }
}