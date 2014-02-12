using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MapAPI.Models;

namespace MapAPI.Controllers
{
    /* api/cities */
    public class CitiesController : ApiController
    {
        private CityService _cityService;

        public CitiesController()
        {
            this._cityService = new CityService();
        }

        /* api/cities : Returns all cities */
        public IEnumerable<CityContract> GetAllCities()
        {
            try
            {
                return this._cityService.GetAllCities();
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }
        }

        /* api/cities/{input} : Returns city with id {input} or cities with name that contains {input} */
        [HttpGet]
        public IEnumerable<CityContract> GetCity(string input)
        {
            IEnumerable<CityContract> cities = null;
            try
            {
                cities = this._cityService.GetCity(input);
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }

            if (cities == null || cities.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("No cities found.");
                throw new HttpResponseException(message);
            }

            return cities;
        }

        /* api/cities/{id}/areas : Returns areas with city_id {id} */
        [HttpGet]
        public IEnumerable<AreaContract> Areas(string id)
        {
            int i;
            if (!int.TryParse(id, out i))
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.Content = new StringContent(String.Format("Parameter '{0}' not accepted as id.", id));
                throw new HttpResponseException(message);
            }
            IEnumerable<AreaContract> ret = this._cityService.GetAreasByCityID(i);
            if (ret.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent(String.Format("No city with id '{0}'.", id));
                throw new HttpResponseException(message);
            }

            return ret;
        }

        /* api/cities/{id}/locations : Returns locations with city_id {id} */
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
            IEnumerable<LocationContract> ret = this._cityService.GetLocationsByCityId(i);
            if (ret.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent(String.Format("No city with id '{0}'.", id));
                throw new HttpResponseException(message);
            }

            return ret;
        }
    }
}
