using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using WebTextEditor.Infrastructure;

namespace WebTextEditor
{
    /// <summary>
    ///     Web API configuration.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Perform api configuration.
        /// </summary>
        /// <param name="config">Configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Configure services
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}