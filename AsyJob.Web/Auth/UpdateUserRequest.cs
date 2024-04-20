namespace AsyJob.Web.Auth
{
    public record UpdateUserRequest(
        string? Email = null,
        string? Username = null,
        IEnumerable<string>? Rights = null
    );
}