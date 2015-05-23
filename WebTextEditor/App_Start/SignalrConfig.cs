using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Owin;
using WebTextEditor.Infrastructure;

namespace WebTextEditor
{
    /// <summary>
    ///     SignalR configuration.
    /// </summary>
    public static class SignalrConfig
    {
        /// <summary>
        ///     Performs registration of SignalR.
        /// </summary>
        /// <param name="app">Application.</param>
        public static void Register(IAppBuilder app)
        {
            var dependencyResolver = IocConfig.GetDependencyResolver();

            // Set camel case for SignalR json serializer
            var settings = new JsonSerializerSettings {ContractResolver = new SignalrContractResolver()};
            var serializer = JsonSerializer.Create(settings);
            dependencyResolver.Register(typeof (JsonSerializer), () => serializer);

            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,
                Resolver = dependencyResolver
            };

            app.MapSignalR(hubConfiguration);

            GlobalHost.DependencyResolver = dependencyResolver;
        }
    }
}