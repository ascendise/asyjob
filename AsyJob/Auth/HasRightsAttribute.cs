using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace AsyJob.Auth
{
    /// <summary>
    /// Sets a policy in the format "{<see cref="POLICY_PREFIX"/>}_{<see cref="Right.ToString"></see>}
    /// </summary>
    internal class HasRightsAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "HasRights";

        public Right[] Right
        {
            set
            {
                Policy = ToPolicy(value);
            }
        }

        private static string ToPolicy(Right[] rights)
        {
            var rightString = string.Join("_", rights.Select(x => x.ToString()));
            return $"{POLICY_PREFIX}_{rightString}";
        }
    }
}
