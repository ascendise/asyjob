using System.Security.Cryptography.X509Certificates;

namespace AsyJob.Lib.Client.Abstract.Users
{
    public record UserResponse(Guid Id, string Username, IEnumerable<string> Rights);
}