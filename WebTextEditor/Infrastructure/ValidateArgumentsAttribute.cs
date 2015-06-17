using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace WebTextEditor.Infrastructure
{
    /// <summary>
    ///     Performs arguments validation of controller methods.
    /// </summary>
    public sealed class ValidateArgumentsAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ValidateArgumentsAttribute()
        {
            BadRequestText = "Bad request.";
        }

        /// <summary>
        ///     Error text message.
        /// </summary>
        public string BadRequestText { get; set; }

        /// <summary>
        ///     Executes arguments validation.
        /// </summary>
        /// <param name="actionContext">Context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (actionContext.ActionArguments.Any(p => p.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, BadRequestText);
            }

            var modelState = actionContext.ModelState;
            if (modelState.IsValid)
            {
                return;
            }

            var modelDictionary = new ModelStateDictionary();
            foreach (var kv in modelState.Where(kv => kv.Value.Errors.Count != 0))
            {
                modelDictionary.AddModelError(kv.Key.Split('.').Last(), kv.Value.Errors.First().ErrorMessage);
            }

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelDictionary);
        }
    }
}