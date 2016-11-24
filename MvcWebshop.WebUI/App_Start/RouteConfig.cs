using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcWebshop.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "orderDetail",
                url: "order/{id}",
                defaults: new { controller = "Orders", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "breweryDetail",
                url: "brewery/{id}",
                defaults: new { controller = "Manufacturers", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "beerDetail",
                url: "beer/{id}",
                defaults: new { controller = "BeerProducts", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "cart",
                url: "cart/{action}/{id}",
                defaults: new { controller = "CartLines", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "breweries",
                url: "breweries/{action}/{id}",
                defaults: new { controller = "Manufacturers", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "beers",
                url: "beers/{action}/{id}",
                defaults: new { controller = "BeerProducts", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "orders",
                url: "orders/{action}/{id}",
                defaults: new { controller = "Orders", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "review",
                url: "review",
                defaults: new { controller = "BeerReviews", action = "Create" }
            );

            routes.MapRoute(
                name: "adminOrders",
                url: "admin/orders/{action}/{id}",
                defaults: new { controller = "Orders", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "adminBeers",
                url: "admin/beers/{action}/{id}",
                defaults: new { controller = "BeerProducts", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "adminReviews",
                url: "admin/reviews/{action}/{id}",
                defaults: new { controller = "BeerReviews", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "adminBreweries",
                url: "admin/breweries/{action}/{id}",
                defaults: new { controller = "Manufacturers", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
