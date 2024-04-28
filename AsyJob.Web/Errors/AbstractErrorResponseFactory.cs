namespace AsyJob.Web.Errors
{
    internal abstract class AbstractErrorResponseFactory(IEnumerable<Type> supportedTypes) : IErrorResponseFactory
    {
        public IEnumerable<Type> SupportedTypes { get; }  = supportedTypes;

        public abstract ErrorResponse Create(Exception ex);

        public bool Supports(Exception ex)
            => SupportedTypes.Any(t => t == ex.GetType());
    }
}

