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
        IBans bans,
        User? user = null) : IUserManager
    {
        private readonly IAuthorizationManager _authManager = authManager;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IWhitelist _whitelist = whitelist;
        private readonly IBans _bans = bans;
        private readonly User? _user = user;

        public Task Ban(string email)
        {
            _authManager.AuthenticatedContext(() =>
            {
                _bans.Ban(email);
            }, _user, [new Right("Users", Operation.Write)]);
            return Task.CompletedTask;
        }

        public Task Whitelist(string email)
        {
            _authManager.AuthenticatedContext(() =>
            {
                _whitelist.Add(email);
            }, _user, [new Right("Users", Operation.Write)]);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(Guid userId, UserUpdate user)
        {
            throw new NotImplementedException();
        }

    }
}
