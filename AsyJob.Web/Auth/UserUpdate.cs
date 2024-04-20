using AsyJob.Lib.Auth;

namespace AsyJob.Web.Auth
{
    public record UserUpdate(
        string? Email, 
        string? Username, 
        IEnumerable<Right> Rights
    );
}