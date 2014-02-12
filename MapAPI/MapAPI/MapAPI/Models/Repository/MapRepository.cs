using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MapAPI.Models
{
    public class MapRepository : IMapRepository
    {
        private MapEntities m_entities = new MapEntities();

        public MapRepository()
        {
            /* Resolves seralization issues when returning xml and json */
            this.m_entities.Configuration.ProxyCreationEnabled = false;
            this.m_entities.Configuration.LazyLoadingEnabled = true;
        }

        public T Get<T>(Func<T, bool> where) where T : class
        {
            return this.m_entities.Set<T>().SingleOrDefault(where);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return this.m_entities.Set<T>().AsEnumerable();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.m_entities.Set<T>().AsQueryable<T>();
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return this.m_entities.Set<Location>().Include(l => l.LocationNames).AsEnumerable();
        }

        public IEnumerable<Location> GetAllLocationsWithAreaAndCity()
        {
            return this.m_entities.Set<Location>()
                .Include(l => l.LocationNames)
                .Include(l => l.Area)
                .Include(l => l.Area.City)
                .Include(l => l.LocationType)
                .AsEnumerable();
        }
        
        public IEnumerable<Area> GetAllAreasWithCity()
        {
            return this.m_entities.Set<Area>().Include(a => a.City).AsEnumerable();
        }

        public IEnumerable<Location> GetLocationsByName(string input)
        {
            return GetAllLocations()
                .Where(l => l.LocationNames.ToList()
                    .Where(
                        ln => ln.location_name_eng.Contains(input) ||
                        ln.location_name_swe.Contains(input)
                    ).Count() > 0).ToList();
        }

        public IEnumerable<Location> GetLocationsByIdOrName(string input, int id)
        {
            return GetAllLocations()
                .Where(l => l.LocationNames.ToList()
                    .Where(
                        ln => ln.location_name_eng.Contains(input) ||
                        ln.location_name_swe.Contains(input)
                    ).Count() > 0 ||
                    l.location_id == id
                ).ToList();
        }

        public IEnumerable<Location> GetLocationsByAreaId(int id)
        {
            return this.m_entities.Set<Location>()
                .Include(l => l.LocationNames)
                .Where(l => l.area_id == id);
        }

        public IEnumerable<LocationType> GetAllLocationTypes()
        {
            return this.m_entities.Set<LocationType>().Include(lt => lt.Icon);
        }

        public IEnumerable<LocationType> GetLocationTypesByName(string input)
        {
            return GetAllLocationTypes()
                .Where(
                    lt => lt.location_type_eng.Contains(input) ||
                    lt.location_type_swe.Contains(input)
                ).ToList();
        }

        public IEnumerable<LocationType> GetLocationTypesByIdOrName(string input, int id)
        {
            return GetAllLocationTypes()
                .Where(
                    lt => lt.location_type_eng.Contains(input) ||
                    lt.location_type_swe.Contains(input) ||
                    lt.location_type_id == id
                ).ToList();
        }

        public IEnumerable<Location> GetLocationsByLocationTypeId(int id)
        {
            return this.m_entities.Set<Location>()
                .Include(l => l.LocationNames)
                .Where(l => l.location_type_id == id);
        }
    }
}