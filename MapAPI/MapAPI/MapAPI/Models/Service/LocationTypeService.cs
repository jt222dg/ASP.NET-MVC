using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAPI.Models
{
    public class LocationTypeService
    {
        private IMapRepository _repository;

        public LocationTypeService()
            :this (new MapRepository())
        {}

        public LocationTypeService(IMapRepository repository)
        {
            this._repository = repository;
        }
        public IEnumerable<LocationTypeContract> GetAllLocationTypes()
        {
            /* Get all location types */
            return LocationTypeContract.ToContracts(this._repository.GetAllLocationTypes());
        }

        public IEnumerable<LocationTypeContract> GetLocationType(string input)
        {
            int id;
            IEnumerable<LocationType> locationTypes = new List<LocationType>();

            /* Get location name by id or location names by name */
            if (int.TryParse(input, out id))
            {
                return LocationTypeContract.ToContracts(this._repository.GetLocationTypesByIdOrName(input, id));
            }
            else
            {
                return LocationTypeContract.ToContracts(this._repository.GetLocationTypesByName(input));
            }
        }

        public IEnumerable<LocationContract> GetLocationsByLocationTypeID(int id)
        {
            /* Return locations as contracts */
            return LocationContract.ToContracts(this._repository.GetLocationsByLocationTypeId(id));
        }
    }
}