﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class MapEntities : DbContext
    {
        public MapEntities()
            : base("name=MapEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationName> LocationNames { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
    
        public virtual ObjectResult<Location> usp_area_locations(Nullable<int> area_id)
        {
            var area_idParameter = area_id.HasValue ?
                new ObjectParameter("area_id", area_id) :
                new ObjectParameter("area_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Location>("usp_area_locations", area_idParameter);
        }
    
        public virtual ObjectResult<Location> usp_area_locations(Nullable<int> area_id, MergeOption mergeOption)
        {
            var area_idParameter = area_id.HasValue ?
                new ObjectParameter("area_id", area_id) :
                new ObjectParameter("area_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Location>("usp_area_locations", mergeOption, area_idParameter);
        }
    
        public virtual ObjectResult<Location> usp_search_locations(string search)
        {
            var searchParameter = search != null ?
                new ObjectParameter("search", search) :
                new ObjectParameter("search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Location>("usp_search_locations", searchParameter);
        }
    
        public virtual ObjectResult<Location> usp_search_locations(string search, MergeOption mergeOption)
        {
            var searchParameter = search != null ?
                new ObjectParameter("search", search) :
                new ObjectParameter("search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Location>("usp_search_locations", mergeOption, searchParameter);
        }
    }
}