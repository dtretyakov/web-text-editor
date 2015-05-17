using Microsoft.AspNet.SignalR;
using Owin;

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
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,
                Resolver = IocConfig.GetDependencyResolver()
            };

            app.MapSignalR(hubConfiguration);
        }
    }
}