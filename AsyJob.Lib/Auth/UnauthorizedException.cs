using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Auth
{
    public class UnauthorizedException(User? user, IEnumerable<Right> missingRights) : Exception("User does not have all required rights")
    {
        public User? User { get; } = user;
        public IEnumerable<Right> MissingRights { get; } = missingRights;
    }
}
