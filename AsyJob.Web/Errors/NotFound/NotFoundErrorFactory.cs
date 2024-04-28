
using System.Net;

namespace AsyJob.Web.Errors.NotFound
{
    internal class NotFoundErrorFactory() : AbstractErrorResponseFactory([typeof(KeyNotFoundException)])
    {
        protected override HttpStatusCode OnCreate(Exception ex)
            => HttpStatusCode.NotFound;
    }
}
