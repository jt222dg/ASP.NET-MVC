using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(Location_Type_Metadata))]
    [DisplayName("Typ")]
    public partial class LocationType
    {
    }

    [Serializable]
    public class Location_Type_Metadata
    {
        [DisplayName("Namn")]
        [Required(ErrorMessage = "Vänligen ange ett namn")]
        public string location_type_swe { get; set; }

        [DisplayName("Översättning(ENG)")]
        [Required(ErrorMessage = "Vänligen ange en översättning")]
        public string location_type_eng { get; set; }

        [DisplayName("Ikon")]
        [Required(ErrorMessage = "Vänligen ange en ikon")]
        public byte icon_id { get; set; }

    }
}