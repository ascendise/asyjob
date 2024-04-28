namespace AsyJob.Web.Errors
{
    internal record ErrorResponse(
        string Title,
        string Description,
        int StatusCode
    );
}
