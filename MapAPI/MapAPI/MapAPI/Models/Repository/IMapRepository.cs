using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAPI.Models
{
    public interface IMapRepository
    {
        T Get<T>(Func<T, bool> where) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<Location> GetAllLocations();
        IEnumerable<Location> GetAllLocationsWithAreaAndCity();
        IEnumerable<Area> GetAllAreasWithCity();
        IEnumerable<Location> GetLocationsByName(string input);
        IEnumerable<Location> GetLocationsByIdOrName(string input, int id);
        IEnumerable<Location> GetLocationsByAreaId(int id);
        IEnumerable<LocationType> GetAllLocationTypes();
        IEnumerable<LocationType> GetLocationTypesByName(string input);
        IEnumerable<LocationType> GetLocationTypesByIdOrName(string input, int id);
        IQueryable<T> Query<T>() where T : class;

        IEnumerable<Location> GetLocationsByLocationTypeId(int id);
    }
}
