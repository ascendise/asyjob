using AsyJob.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Tests.TestDoubles
{
    internal class FakeUserStore(List<User>? initUsers = null) : IUserStore<User>
    {
        private readonly List<User> _users = initUsers ?? [];
        public IEnumerable<User> Users { get => _users; }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            _users.Add(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _users.Remove(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            _users.Clear();
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult(
                    _users.SingleOrDefault(u => u.Id == Guid.Parse(userId))
                );

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult(
                    _users.SingleOrDefault(u => u.NormalizedUserName == normalizedUserName)
                );

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Dont need it right now and can't be bothered to write it.");
        }
    }
}
