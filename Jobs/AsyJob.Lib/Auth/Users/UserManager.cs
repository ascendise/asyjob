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
        User? user = null) : IUserManager
    {
        private readonly IAuthorizationManager _authManager = authManager;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly User? _user = user;

        public async Task<IEnumerable<User>> GetAll()
            => await _authManager.AuthenticatedContext(async () =>
                {
                    return await _userRepo.GetAll();
                }, _user, [new Right("Users", Operation.Read)]);

        public async Task<User> Update(Guid userId, UserUpdate update)
            => await _authManager.AuthenticatedContext(async () =>
            {
                var oldUser = await _userRepo.Get(userId)
                    ?? throw new KeyNotFoundException();
                var newUser = new User(
                    userId,
                    update.Username ?? oldUser.Username,
                    update.Rights ?? oldUser.Rights,
                    oldUser.Active
                );
                return await _userRepo.Update(newUser);
            }, _user, [new Right("Users", Operation.Read | Operation.Write)]);
    }
}
