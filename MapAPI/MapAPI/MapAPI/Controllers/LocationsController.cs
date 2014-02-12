using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MapAPI.Models;

namespace MapAPI.Controllers
{
    /* api/locaitons */
    public class LocationsController : ApiController
    {
        private LocationService _locationService;

        public LocationsController()
        {
            this._locationService = new LocationService();
        }

        /* api/locations : Returns all locaitons */
        public IEnumerable<LocationContract> GetAllLocations()
        {
            try
            {
                return this._locationService.GetAllLocations();
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }
        }

        /* api/locations/{input} : Returns location with id {input} or locaitons with name that contains {input} */
        [HttpGet]
        public IEnumerable<LocationContract> GetLocation(string input)
        {
            IEnumerable<LocationContract> locations = null;
            try
            {
                locations = this._locationService.GetLocation(input);
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }

            if (locations == null || locations.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("No locations found.");
                throw new HttpResponseException(message);
            }

            return locations;
        }
    }
}
