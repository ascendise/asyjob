using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Lib.Client.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace AsyJob.Web.Auth
{
    /// <summary>
    /// Checks if user is on whitelist and not banend, else refuses access
    /// </summary>
    public class VipAuthorizationHandler(IUsersApi usersApi) : IAuthorizationHandler
    {
        private readonly IUsersApi _usersApi = usersApi;

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var email = context.User?.FindFirst(ClaimTypes.Email)?.Value;
            if(email is null || !await IsAllowed(email))
            {
                context.Fail(new AuthorizationFailureReason(this, "User was either banned or removed from whitelist"));
            }
        }

        private async Task<bool> IsAllowed(string email) 
        {
            var access = await _usersApi.GetUserAccessRights(new(email));
            return access.IsWhitelisted && !access.IsBanned;
        }
    }
}
