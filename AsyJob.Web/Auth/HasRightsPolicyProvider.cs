using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AsyJob.Web.Auth
{
    internal class HasRightsPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(
                new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build()
            );

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => Task.FromResult<AuthorizationPolicy?>(null);

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var rights = policyName.Split(':')[1]
                .Split(';')
                .Select(s => new Right(s))
                .ToArray();
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new HasRightsRequirement(rights))
                .Build();
            return Task.FromResult(policy)!;
        }
    }
}
