using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MapAPI.Models
{
    /* City as a datacontract */
    [DataContract(Namespace = "", Name = "City")]
    public class CityContract
    {
        /* Default constructor */
        public CityContract()
        {
            this.CityId = -1;
            this.Name = "";
            this.Latitude = Decimal.Parse("1");
            this.Longitude = Decimal.Parse("1");
        }

        /* Constructor with a city */
        public CityContract(City c)
        {
            this.CityId = c.city_id;
            this.Name = c.city1;
            this.Latitude = c.latitude;
            this.Longitude = c.longitude;
        }

        /* City to City contract */
        public static CityContract ToContract(City c)
        {
            return new CityContract(c);
        }

        /* City to City contracts */
        public static IEnumerable<CityContract> ToContracts(IEnumerable<City> cities)
        {
            List<CityContract> ccs = new List<CityContract>();

            foreach (City city in cities)
                ccs.Add(CityContract.ToContract(city));

            return ccs.AsEnumerable();
        }

        [DataMember(Name = "City_ID")]
        public int CityId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [DataMember(Name = "Longitude")]
        public decimal? Longitude { get; set; }
    }
}