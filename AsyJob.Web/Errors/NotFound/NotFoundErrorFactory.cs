
namespace AsyJob.Web.Errors.NotFound
{
    internal class NotFoundErrorFactory() : AbstractErrorResponseFactory([typeof(KeyNotFoundException)])
    {
        protected override ErrorResponse OnCreate(Exception ex)
            => new("Not Found", ex.Message, 404);
    }
}
