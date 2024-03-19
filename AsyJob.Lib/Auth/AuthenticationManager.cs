﻿using System;
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
            missingRights = user.Needs(requiredRights);
            return !missingRights.Any();
        }
    }
}
