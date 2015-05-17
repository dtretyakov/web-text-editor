using System.Web.Mvc;
using System.Web.Routing;

namespace WebTextEditor
{
    /// <summary>
    ///     Routes configuration.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        ///     Performs MVC routes registration.
        /// </summary>
        /// <param name="routes">Routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional});
        }
    }
}