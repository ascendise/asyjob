using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web.Auth
{
    public interface IAspUserManager
    {
        public IEnumerable<User> Users { get; }

        public Task<User?> FindByIdAsync(Guid guid);
        public Task<IdentityResult> UpdateAsync(User user);

    }
}
