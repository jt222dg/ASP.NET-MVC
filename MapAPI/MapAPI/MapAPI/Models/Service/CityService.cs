using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAPI.Models
{
    public class CityService
    {
        private IMapRepository _repository;
        private AreaService _areaService;

        public CityService()
            :this (new MapRepository())
        {
        }

        public CityService(IMapRepository repository)
        {
            this._repository = repository;
            this._areaService = new AreaService();
        }

        public IEnumerable<CityContract> GetAllCities()
        {
            return CityContract.ToContracts(this._repository.GetAll<City>());
        }

        public IEnumerable<CityContract> GetCity(string input)
        {
            int id;
            IEnumerable<City> cities;

            /* Get city by id or cities by name */
            if (int.TryParse(input, out id))
                cities = this._repository.Query<City>().Where(c => c.city1.Contains(input) || c.city_id == id);
            else
                cities = this._repository.Query<City>().Where(c => c.city1.Contains(input));

            return CityContract.ToContracts(cities);
        }

        public IEnumerable<AreaContract> GetAreasByCityID(int id)
        {
            /* Get city */
            City city = this._repository.Get<City>(c => c.city_id == id);

            /* Get the cities areas */
            IEnumerable<Area> areas = this._repository.Query<Area>().Where(c => c.city_id == id);

            /* Return locations as contracts */
            return AreaContract.ToContracts(areas);
        }

        public IEnumerable<LocationContract> GetLocationsByCityId(int id)
        {
            List<Location> locations = new List<Location>();

            /* Get locations */
            List<short> areaIds = 
                this._repository.Query<Area>()
                .Where(a => a.city_id == id)
                .Select(a => a.area_id)
                .ToList();

            areaIds.ForEach(aId =>
                this._repository.GetLocationsByAreaId(aId).ToList().ForEach(l => locations.Add(l))
            );

            /* Return locations as contracts */
            return LocationContract.ToContracts(locations);
        }
    }
}