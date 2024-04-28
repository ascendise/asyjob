using Microsoft.AspNetCore.Diagnostics;
using MongoDB.Driver.Core.Operations;

namespace AsyJob.Web.Errors
{
    internal class ExceptionHandler(ErrorResponseFactory errorFactory) : IExceptionHandler
    {
        private readonly ErrorResponseFactory _errorFactory = errorFactory;

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (!_errorFactory.Supports(exception))
                return ValueTask.FromResult(false);
            var statusCode = _errorFactory.Create(exception);
            httpContext.Response.StatusCode = (int)statusCode;
            return ValueTask.FromResult(true);
        }
    }
}
