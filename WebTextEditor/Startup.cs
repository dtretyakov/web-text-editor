using Microsoft.Owin;
using Owin;
using WebTextEditor;

[assembly: OwinStartup(typeof (Startup))]

namespace WebTextEditor
{
    /// <summary>
    ///     Application startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        ///     Configures application.
        /// </summary>
        /// <param name="app">Application instance.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            SignalrConfig.Register(app);
        }
    }
}