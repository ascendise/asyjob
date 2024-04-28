namespace AsyJob.Web.Errors
{
    internal class InternalServerErrorFactory() : AbstractErrorResponseFactory([typeof(Exception)])
    {
        protected override ErrorResponse OnCreate(Exception ex)
            => new("Internal Server Error", "Server could not complete process because of an unhandled error", 500);
    }
}
