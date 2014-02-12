using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(Location_Metadata))]
    [DisplayName("Plats")]
    public partial class Location
    {
        public void BuildLocationNames(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                LocationNames.Add(new LocationName());
            }
        }
    }

    [Serializable]
    public class Location_Metadata
    {
        [DisplayName("Våning")]
        [Required]
        public Nullable<byte> floor_nr { get; set; }

        [Required]
        public byte location_type_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N6}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Du måste ange en unik position")]
        public Nullable<decimal> latitude { get; set; }

        [DisplayFormat(DataFormatString = "{0:N6}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> longitude { get; set; }
    }
}