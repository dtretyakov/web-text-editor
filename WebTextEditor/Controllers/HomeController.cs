using System.Web.Mvc;

namespace WebTextEditor.Controllers
{
    /// <summary>
    ///     Front page controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        ///     Returns a SPA.
        /// </summary>
        [Route]
        [Route("documents")]
        [Route("documents/{id}")]
        public ActionResult Index()
        {
            return View();
        }
    }
}