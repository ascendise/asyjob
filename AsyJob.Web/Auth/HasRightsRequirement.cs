using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AsyJob.Web.Auth
{
    public class HasRightsRequirement(Right[] rights) : IAuthorizationRequirement
    {
        public Right[] Rights { get; } = rights;
    }
}
