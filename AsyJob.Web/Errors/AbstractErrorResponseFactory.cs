using System.Net;

namespace AsyJob.Web.Errors
{
    internal abstract class AbstractErrorResponseFactory(IEnumerable<Type> supportedTypes) : IErrorResponseFactory
    {
        public IEnumerable<Type> SupportedTypes { get; } = supportedTypes;

        public HttpStatusCode Create(Exception ex)
        {
            if (!Supports(ex))
                throw new ArgumentException("Unsupported type", nameof(ex));
            return OnCreate(ex);
        }

        /// <summary>
        /// Called in <see cref="Create(Exception)"/> after input validation.
        /// <paramref name="ex"/> is sure to be a supported type
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected abstract HttpStatusCode OnCreate(Exception ex);

        public bool Supports(Exception ex)
            => SupportedTypes.Any(t => t == ex.GetType());
    }
}

