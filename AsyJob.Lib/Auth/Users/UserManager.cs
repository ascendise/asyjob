using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Auth.Users
{
    public class UserManager(
        IAuthorizationManager authManager,
        IUserRepository userRepo,
        IWhitelist whitelist,
        IBans bans) : IUserManager
    {
        private readonly IAuthorizationManager _authManager = authManager;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IWhitelist _whitelist = whitelist;
        private readonly IBans _bans = bans;

        public void Ban(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User Update(Guid userId, UserUpdate user)
        {
            throw new NotImplementedException();
        }

        public void Whitelist(string email)
        {
            throw new NotImplementedException();
        }
    }
}
