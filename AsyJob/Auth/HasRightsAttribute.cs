using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AsyJob.Auth
{
    /// <summary>
    /// Sets a policy in the format "HasRights_{<see cref="Right.ToString"></see>}
    /// </summary>
    internal class HasRightsAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "HasRights";

        public Right Right
        {
            set
            {
                Policy = ToPolicy(value);
            }
        }

        private static string ToPolicy(Right right)
            => $"{POLICY_PREFIX}_{right}";
    }
}
