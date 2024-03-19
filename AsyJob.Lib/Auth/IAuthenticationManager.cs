using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace AsyJob.Lib.Auth
{
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Used to run a procedure in an authenticated context (a present user)
        /// If the user does not have the required rights, the method throws a <see cref="SecurityException"/>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <param name="requiredRights"></param>
        /// <exception cref="SecurityException"></exception>
        void AuthenticatedContext(Action action, User user, IEnumerable<Right> requiredRights);
    }

}
