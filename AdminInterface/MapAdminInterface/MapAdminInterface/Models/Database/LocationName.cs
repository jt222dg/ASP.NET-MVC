//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapAdminInterface.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LocationName
    {
        public int location_name_id { get; set; }
        public int location_id { get; set; }
        public string location_name_swe { get; set; }
        public string location_name_eng { get; set; }
        public bool is_main { get; set; }
    
        public virtual Location Location { get; set; }
    }
}