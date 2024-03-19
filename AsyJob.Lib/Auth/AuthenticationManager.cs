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
            foreach(var requiredRight in requiredRights)
            {
                Right? userRight = user.Rights.SingleOrDefault(r => r.Resource == requiredRight.Resource);
                if(!userRight.HasValue)
                {
                    missing.Add(requiredRight);
                    continue;
                }
                var missingOps = ~userRight.Value.Ops & requiredRight.Ops;
                if(missingOps > 0)
                {
                    missing.Add(new Right(requiredRight.Resource, missingOps)); 
                }
            }
            missingRights = missing;
            return missing.Count == 0;
        }
    }
}
