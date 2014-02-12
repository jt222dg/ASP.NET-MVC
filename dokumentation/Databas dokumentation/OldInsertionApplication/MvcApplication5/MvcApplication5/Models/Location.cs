using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication5.Models
{
    public class Location
    {
        public int? typ { get; set;}
        public decimal? latitud { get; set;}
        public decimal? longitud { get; set;}
        public string ritningsnamn { get; set;}
        public string rumsnamn2 { get; set;}
        public string rumsnamn3 { get; set;}
        public string rumsnamn4 { get; set;}
        public string byggnad_sv { get; set;}
        public string byggnad_en { get; set;}
        public byte? vaning { get; set;}
        public string stad { get; set;}
        public string rumstyp_sv { get; set;}
        public string rumstyp_en { get; set;}
    }
}