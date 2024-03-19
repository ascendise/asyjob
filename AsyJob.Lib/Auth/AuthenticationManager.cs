using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Auth
{
    public class AuthenticationManager : IAuthenticationManager
    {
        public void AuthenticatedContext(Action action, User user, IEnumerable<Right> requiredRights)
        {
            if(!HasPermission(user, requiredRights, out var missingRights))
            {
                throw new UnauthorizedException(user, missingRights);
            } 
            action();
        }

        private static bool HasPermission(User user, IEnumerable<Right> requiredRights, out IEnumerable<Right> missingRights)
        {
            List<Right> missing = [];
            foreach(var right in requiredRights)
            {
                Right? userRight = user.Rights.SingleOrDefault(r => r.Resource == right.Resource);
                if(!userRight.HasValue)
                {
                    missing.Add(right);
                    continue;
                }
                var missingOps = userRight.Value.Ops ^ right.Ops;
                if(missingOps > 0)
                {
                    missing.Add(new Right(right.Resource, missingOps)); 
                }
            }
            missingRights = missing;
            return missing.Count == 0;
        }
    }
}
