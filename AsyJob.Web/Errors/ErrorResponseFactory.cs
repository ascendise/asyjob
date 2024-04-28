using Amazon.Runtime;

namespace AsyJob.Web.Errors
{
    internal class ErrorResponseFactory(IEnumerable<IErrorResponseFactory> errorResponseFactories) 
        : AbstractErrorResponseFactory(errorResponseFactories.SelectMany(erf => erf.SupportedTypes))
    {
        private readonly IEnumerable<IErrorResponseFactory> _errorResponseFactories = errorResponseFactories;

        public override ErrorResponse Create(Exception ex)
        {
            var factory = _errorResponseFactories.SingleOrDefault(f => f.Supports(ex))
                ?? throw new ArgumentException("Exception is not supported", nameof(ex));
            return factory.Create(ex);
        }
    }
}

