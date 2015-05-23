using System.Web.Optimization;

namespace WebTextEditor
{
    /// <summary>
    ///     Bundles configuration.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        ///     For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        /// <param name="bundles">Bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            bundles.ResetAll();

            bundles.Add(new ScriptBundle("~/assets/js/libs").Include(
                "~/scripts/angular.js",
                "~/scripts/angular-route.js",
                "~/scripts/angular-resource.js",
                "~/scripts/angular-animate.js",
                "~/scripts/angular-elastic.js",
                "~/scripts/angular-modal-service.js",
                "~/scripts/toastr.js",
                "~/scripts/jquery.signalR-{version}.js",
                "~/scripts/EventEmitter.js",
                "~/scripts/logoot.js",
                "~/scripts/logoot-text.js",
                "~/scripts/textarea-caret-position.js"));

            bundles.Add(new ScriptBundle("~/assets/js/editor").Include(
                "~/content/js/editor.module.js",
                "~/content/js/editor.auth.js",
                "~/content/js/editor.config.js",
                "~/content/js/editor.routes.js",
                "~/content/js/directives/*.js",
                "~/content/js/factories/*.js",
                "~/content/js/services/*.js",
                "~/content/js/controllers/*.js"));

            bundles.Add(new ScriptBundle("~/assets/js/modernizr").Include(
                "~/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/assets/js/bootstrap").Include(
                "~/scripts/jquery-{version}.js",
                "~/scripts/bootstrap.js",
                "~/scripts/respond.js"));

            bundles.Add(new StyleBundle("~/assets/css/styles").Include(
                "~/content/css/bootstrap.css",
                "~/content/css/site.css",
                "~/content/css/toastr.css"));
        }
    }
}