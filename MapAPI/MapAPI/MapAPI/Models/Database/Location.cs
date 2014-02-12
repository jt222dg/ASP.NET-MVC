//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        public Location()
        {
            this.LocationNames = new HashSet<LocationName>();
        }
    
        public int location_id { get; set; }
        public short location_type_id { get; set; }
        public short area_id { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public Nullable<byte> floor_nr { get; set; }
    
        public virtual Area Area { get; set; }
        public virtual LocationType LocationType { get; set; }
        public virtual ICollection<LocationName> LocationNames { get; set; }
    }
}
