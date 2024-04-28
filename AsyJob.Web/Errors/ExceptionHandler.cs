using Microsoft.AspNetCore.Diagnostics;
using MongoDB.Driver.Core.Operations;

namespace AsyJob.Web.Errors
{
    internal class ExceptionHandler(ErrorResponseFactory errorFactory) : IExceptionHandler
    {
        private readonly ErrorResponseFactory _errorFactory = errorFactory;

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(exception is KeyNotFoundException)
            {
                httpContext.Response.StatusCode = 404;
                return ValueTask.FromResult(false);
            }
            return ValueTask.FromResult(false);
        }
    }
}
