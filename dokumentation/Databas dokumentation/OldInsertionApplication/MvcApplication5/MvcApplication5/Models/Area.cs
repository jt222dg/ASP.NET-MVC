using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication5.Models
{
    public class Area
    {
        public int? typ { get; set;}
        public decimal? latitud { get; set;}
        public decimal? longitud { get; set;}
        public string byggnad_sv { get; set; }
        public string byggnad_en { get; set; }
        public string stad { get; set;}
    }
}