using AsyJob.Lib.Auth;

namespace AsyJob.Web.Auth
{
    public record UpdateUserRequest(
        string? Email, 
        string? Username, 
        IEnumerable<Right> Rights
    );
}