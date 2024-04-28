using System.Net;

namespace AsyJob.Web.Errors
{
    internal class InternalServerErrorFactory() : AbstractErrorResponseFactory([typeof(Exception)])
    {
        protected override HttpStatusCode OnCreate(Exception ex)
            => HttpStatusCode.InternalServerError;
    }
}
