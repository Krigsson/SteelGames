using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SteelGames
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GameDetails",
                url: "GameDetails/{id}",
                defaults: new { controller = "Home", action = "Details", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Registration",
                url: "Register",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                );

            routes.MapRoute(
               name: "AccountDetails",
               url: "AccountDetails",
               defaults: new { controller = "Account", action = "AccountDetails", id = UrlParameter.Optional }
               );
        }
    }
}
