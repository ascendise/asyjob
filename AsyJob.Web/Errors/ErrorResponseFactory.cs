using System.Net;

namespace AsyJob.Web.Errors
{
    internal class ErrorResponseFactory(IEnumerable<IErrorResponseFactory> errorResponseFactories)
        : AbstractErrorResponseFactory(errorResponseFactories.SelectMany(erf => erf.SupportedTypes))
    {
        private readonly IEnumerable<IErrorResponseFactory> _errorResponseFactories = errorResponseFactories;

        protected override HttpStatusCode OnCreate(Exception ex)
        {
            var factory = _errorResponseFactories.Single(f => f.Supports(ex));
            return factory.Create(ex);
        }
    }
}

