using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MapAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {   
            config.Routes.MapHttpRoute(
                name: "NestedCalls",
                routeTemplate: "{controller}/{id}/{action}"
            );

            config.Routes.MapHttpRoute(
                name: "TableByNameOrId",
                routeTemplate: "{controller}/{input}",
                defaults: new { name = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /* Default return type as JSON */
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
