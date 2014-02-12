using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MapAPI.Models;

namespace MapAPI.Controllers
{
    public class AreasController : ApiController
    {
        private AreaService _areaService;

        public AreasController()
        {
            this._areaService = new AreaService();
        }

        /* api/areas : Returns all areas */
        public IEnumerable<AreaContract> GetAllAreas()
        {
            try
            {
                return this._areaService.GetAllAreas();
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }
        }

        /* api/areas/{input} : Returns area with id {input} or areas with name that contains {input} */
        [HttpGet]
        public IEnumerable<AreaContract> GetArea(string input)
        {
            IEnumerable<AreaContract> areas = null;
            try
            {
                areas = this._areaService.GetArea(input);
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }

            if (areas == null || areas.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("No areas found.");
                throw new HttpResponseException(message);
            }

            return areas;
        }

        /* api/areas/{id}/locations : Returns locations with area_id {id} */
        [HttpGet]
        public IEnumerable<LocationContract> Locations(string id)
        {
            int i;
            if (!int.TryParse(id, out i))
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.Content = new StringContent(String.Format("Parameter '{0}' not accepted as id.", id));
                throw new HttpResponseException(message);
            }
            IEnumerable<LocationContract> ret = this._areaService.GetLocationsByAreaId(i);
            if (ret.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent(String.Format("No area with id '{0}'.", id));
                throw new HttpResponseException(message);
            }

            return ret;
        }
    }
}
