using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(Location_Name_Metadata))]
    partial class LocationName
    {
        public bool Delete { get; set; }
    }
    [Serializable]
    public class Location_Name_Metadata
    {
        [DisplayName("Namn")]
        [Required(ErrorMessage = "Vänligen fyll i ett namn.")]
        public string location_name_swe { get; set; }

        [DisplayName("Översättning(ENG)")]
        public string location_name_eng { get; set; }
    }
}