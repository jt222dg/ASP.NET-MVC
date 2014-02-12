using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MapAPI.Models;

namespace MapAPI.Controllers
{
    /* api/locaitonTypes */
    public class LocationTypesController : ApiController
    {
        private LocationTypeService _locationTypeService;

        public LocationTypesController()
        {
            this._locationTypeService = new LocationTypeService();
        }

        /* api/locationTypes : Returns all locaiton types */
        public IEnumerable<LocationTypeContract> GetAllLocaitonTypes()
        {
            try
            {
                return this._locationTypeService.GetAllLocationTypes();
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }
        }

        /* api/locationTypes/{input} : Returns location type with id {input} or that contains {input} */
        [HttpGet]
        public IEnumerable<LocationTypeContract> GetLocationType(string input)
        {
            IEnumerable<LocationTypeContract> locationTypes = null;
            try
            {
                locationTypes = this._locationTypeService.GetLocationType(input); ;
            }
            catch (Exception)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                message.Content = new StringContent("Unable to connect to server.");
                throw new HttpResponseException(message);
            }

            if (locationTypes == null || locationTypes.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("No Location Types found.");
                throw new HttpResponseException(message);
            }

            return locationTypes;
        }

        /* api/locationtypes/{id}/locations : Returns locations with location_type_id {id} */
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
            IEnumerable<LocationContract> ret = this._locationTypeService.GetLocationsByLocationTypeID(i);
            if (ret.Count() == 0)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent(String.Format("No Location Type with id '{0}'.", id));
                throw new HttpResponseException(message);
            }

            return ret;
        }
    }
}