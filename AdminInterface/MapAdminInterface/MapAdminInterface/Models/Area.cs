using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(Area_Metadata))]
    [DisplayName("Område")]
    public partial class Area
    {
    }

    [Serializable]
    public class Area_Metadata
    {
        [Required(ErrorMessage = "Vänligen välj en stad för området.")]
        public byte city_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N6}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage="Du måste ange en unik position")]
        public Nullable<decimal> latitude { get; set; }

        [DisplayFormat(DataFormatString = "{0:N6}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> longitude { get; set; }

        [Required(ErrorMessage = "Vänligen ange ett namn för området.")]
        
        [DisplayName("Svenskt namn")]
        public string area_swe { get; set; }

        [DisplayName("Översättning(ENG)")]
        public string area_eng { get; set; }
    }
}