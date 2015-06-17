using System.Net;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using WebTextEditor.Domain.Exceptions;

namespace WebTextEditor.Infrastructure
{
    /// <summary>
    ///     Provides response messages according to exception type.
    /// </summary>
    public class CustomExceptionHandler : ExceptionHandler
    {
        /// <summary>
        ///     Executes exception handler.
        /// </summary>
        /// <param name="context">Context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;
            if (exception is NotFoundException)
            {
                context.Result = new NotFoundResult(context.Request);
            }
            else if (exception is ForiddenException)
            {
                context.Result = new StatusCodeResult(HttpStatusCode.Forbidden, context.Request);
            }
            else
            {
                context.Result = new InternalServerErrorResult(context.Request);
            }
        }
    }
}