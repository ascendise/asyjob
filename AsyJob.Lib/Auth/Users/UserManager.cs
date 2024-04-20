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

        public async Task Ban(string email)
            => await _authManager.AuthenticatedContext(async () =>
                {
                    await _bans.Ban(email);
                }, _user, [new Right("Users", Operation.Write)]);

        public async Task Whitelist(string email)
            => await _authManager.AuthenticatedContext(async () =>
                {
                    await _whitelist.Add(email);
                }, _user, [new Right("Users", Operation.Write)]);

        public async Task<IEnumerable<User>> GetAll()
            => await _authManager.AuthenticatedContext(async () =>
                {
                    return await _userRepo.GetAll();
                }, _user, [new Right("Users", Operation.Read)]);

        public Task<User> Update(Guid userId, UserUpdate user)
        {
            throw new NotImplementedException();
        }

    }
}
