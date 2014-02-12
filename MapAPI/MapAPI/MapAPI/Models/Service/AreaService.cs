using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MapAPI.Models
{
    public class AreaService
    {
        private IMapRepository _repository;

        public AreaService()
            :this (new MapRepository())
        {}

        public AreaService(IMapRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<AreaContract> GetAllAreas()
        {
            return AreaContract.ToContracts(this._repository.GetAll<Area>());
        }

        public IEnumerable<AreaContract> GetArea(string input)
        {
            int id;
            IEnumerable<Area> areas;

            /* Get area by id or areas by name */
            if (int.TryParse(input, out id))
            {
                areas = this._repository.Query<Area>()
                    .Where(
                        a => a.area_swe.Contains(input) ||
                        a.area_eng.Contains(input) ||
                        a.area_id == id
                    );
            }
            else
            {
                areas = this._repository.Query<Area>()
                    .Where(
                        a => a.area_swe.Contains(input) ||
                        a.area_eng.Contains(input)
                    );
            }

            return AreaContract.ToContracts(areas);
        }

        public IEnumerable<LocationContract> GetLocationsByAreaId(int id)
        {
            /* Get area by id */
            Area area = this._repository.Get<Area>(a => a.area_id == id);

            /* Get the areas locations */
            IEnumerable<Location> locations = this._repository.GetLocationsByAreaId(id);

            /* Return locations as contracts */
            return LocationContract.ToContracts(locations);
        }
    }
}