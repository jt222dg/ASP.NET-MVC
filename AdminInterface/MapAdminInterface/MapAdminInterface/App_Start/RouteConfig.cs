using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MapAdminInterface
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //MAIN
            routes.MapRoute(
                name: "Om",
                url: "Om",
                defaults: new { controller = "Main", action = "About", id = UrlParameter.Optional }
            );

            //LOCATIONTYPE
            routes.MapRoute(
                name: "SkapaPlatsTyp",
                url: "PlatsTyp/Skapa",
                defaults: new { controller = "LocationType", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "RedigeraPlatsTyp",
               url: "PlatsTyp/Redigera/{id}",
               defaults: new { controller = "LocationType", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TabortPlatsTyp",
                url: "PlatsTyp/Tabort/{id}",
                defaults: new { controller = "LocationType", action = "Delete", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PlatsTyp",
                url: "PlatsTyper",
                defaults: new { controller = "LocationType", action = "Index", id = UrlParameter.Optional }
            );

            //LOCATION
            routes.MapRoute(
                name: "SkapaPlats",
                url: "Plats/Skapa",
                defaults: new { controller = "Location", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "RedigeraPlats",
               url: "Plats/Redigera/{id}",
               defaults: new { controller = "Location", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TabortPlats",
                url: "Plats/Tabort/{id}",
                defaults: new { controller = "Location", action = "Delete", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Plats",
                url: "Platser",
                defaults: new { controller = "Location", action = "Index", id = UrlParameter.Optional }
            );

            //AREA
            routes.MapRoute(
                name: "SkapaOmråde",
                url: "Område/Skapa",
                defaults: new { controller = "Area", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "RedigeraOmråde",
               url: "Område/Redigera/{id}",
               defaults: new { controller = "Area", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TabortOmråde",
                url: "Område/Tabort/{id}",
                defaults: new { controller = "Area", action = "Delete", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Område",
                url: "Områden",
                defaults: new { controller = "Area", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}