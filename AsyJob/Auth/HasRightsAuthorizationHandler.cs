using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AsyJob.Auth
{
    public class HasRightsAuthorizationHandler(IUserStore<User> userStore) : AuthorizationHandler<HasRightsRequirement>
    {
        private readonly IUserStore<User> _userStore = userStore;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRightsRequirement requirement)
        {
            try
            {
                await CheckUserRights(context, requirement);
            }
            catch (AuthorizationFailedException ex) {
                context.Fail(new AuthorizationFailureReason(this, ex.Message));
            }
            context.Succeed(requirement);
        }

        private async Task CheckUserRights(AuthorizationHandlerContext context, HasRightsRequirement requirement)
        {
            var user = await MapToDomainUser(context.User);
            var missingRights = user.Needs(requirement.Rights);
            if (missingRights.Any())
                throw new AuthorizationFailedException("User is missing rights for this action");
        }

        private async Task<Lib.Auth.User> MapToDomainUser(ClaimsPrincipal identity)
        {

            var userId = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                ?? throw new AuthorizationFailedException("User has no id");
            var mongoUser = await _userStore.FindByIdAsync(userId, CancellationToken.None) 
                ?? throw new AuthorizationFailedException("No user with matching id found");
            return mongoUser.GetDomainUser();
        }

        private class AuthorizationFailedException(string message) : Exception(message)
        {

        }
    }
}
