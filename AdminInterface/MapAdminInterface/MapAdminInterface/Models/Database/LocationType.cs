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
    
    public partial class LocationType
    {
        public LocationType()
        {
            this.Locations = new HashSet<Location>();
        }
    
        public short location_type_id { get; set; }
        public string location_type_swe { get; set; }
        public string location_type_eng { get; set; }
        public Nullable<byte> icon_id { get; set; }
    
        public virtual Icon Icon { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
