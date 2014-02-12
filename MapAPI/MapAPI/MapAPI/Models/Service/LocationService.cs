using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAPI.Models
{
    public class LocationService
    {
        private IMapRepository _repository;

        public LocationService()
            :this (new MapRepository())
        {}

        public LocationService(IMapRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<LocationContract> GetAllLocations()
        {
            /* Get all locations */
            IEnumerable<Location> locations = this._repository.GetAllLocations();
            return LocationContract.ToContracts(locations);
        }

        public IEnumerable<LocationContract> GetLocation(string input)
        {
            int id;
            if (int.TryParse(input, out id))
                return LocationContract.ToContracts(this._repository.GetLocationsByIdOrName(input, id));

            return LocationContract.ToContracts(this._repository.GetLocationsByName(input));
        }
    }
}