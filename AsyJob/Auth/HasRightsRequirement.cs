using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AsyJob.Auth
{
    public class HasRightsRequirement(Right[] rights) : IAuthorizationRequirement
    {
        public Right[] Rights { get; } = rights;
    }
}
