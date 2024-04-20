using AsyJob.Lib.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace AsyJob.Web.Auth.Rights
{
    /// <summary>
    /// Sets a policy in the format "{POLICY_PREFIX}:{RIGHT1};{RIGHT2}"
    /// </summary>
    internal class HasRightsAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "HasRights";

        /// <summary>
        /// Expects semicolon-separated values in the same format as <see cref="Right.ToString()"/>
        /// </summary>
        public string Rights
        {
            set
            {
                Policy = ToPolicy(value);
            }
        }

        public HasRightsAttribute(string right) =>
            Rights = right;

        private static string ToPolicy(string rights)
        {
            return $"{POLICY_PREFIX}:{rights}";
        }
    }
}
