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
            var userId = context.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "User has no id"));
                return;
            }
            var mongoUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            if (mongoUser == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "No user with matching id found"));
                return;
            }
            var domainUser = mongoUser.GetDomainUser();
            var missingRights = domainUser.Needs(requirement.Rights);
            if (missingRights.Any())
            {
                context.Fail(new AuthorizationFailureReason(this, "User does not have all rights"));
            } else {
                context.Succeed(requirement);
            }
        }

        private class AuthorizationFailedException(string message) : Exception(message)
        {

        }
    }
}
