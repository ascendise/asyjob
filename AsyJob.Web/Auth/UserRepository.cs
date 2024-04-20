using AsyJob.Lib.Auth.Users;
using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web.Auth
{
    internal class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<Lib.Auth.User?> Get(Guid id)
        {
            var user = FindUser(id);
            if (user is null)
                return null;
            return Map(user);
        }

        private User? FindUser(Guid id)
            => _userManager.Users.SingleOrDefault(u => u.Id == id);

        private static Lib.Auth.User Map(User user) 
            => new(user.Id, user.UserName ?? user.Email ?? user.Id.ToString(), user.Rights);

        public Task<IEnumerable<Lib.Auth.User>> GetAll() 
            => Task.FromResult(_userManager.Users.Select(Map));

        public async Task Remove(Guid id)
        {
            var user = FindUser(id);
            if (user is null)
                return;
            await _userManager.DeleteAsync(user);
        }

        public async Task<Lib.Auth.User> Update(Lib.Auth.User user)
        {
            var dbUser = FindUser(user.Id)
                ?? throw new KeyNotFoundException($"No user with Id {user.Id} found");
            dbUser.Id = user.Id;
            dbUser.UserName = user.Username;
            dbUser.Rights = user.Rights;
            await _userManager.UpdateAsync(dbUser);
        }
    }
}
