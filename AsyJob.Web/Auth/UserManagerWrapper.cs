using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web.Auth
{
    internal class UserManagerWrapper(UserManager<User> userManager) : IAspUserManager
    {
        private readonly UserManager<User> _userManager = userManager;

        public IEnumerable<User> Users => _userManager.Users;

        public Task<User?> FindByIdAsync(Guid guid)
            => _userManager.FindByIdAsync(guid.ToString());

        public Task<IdentityResult> UpdateAsync(User user)
            => _userManager.UpdateAsync(user);
    }
}
