using AsyJob.Lib.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    /// <summary>
    /// Acts without checking for required rights
    /// Useful for tests, that are not authorization related
    /// </summary>
    public class StubAuthorizationManager : IAuthorizationManager
    {
        public void AuthenticatedContext(Action action, User user, IEnumerable<Right> requiredRights)
        {
            action();
        }
    }
}
