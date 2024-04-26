using AsyJob.Web.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Web.Tests.TestDoubles
{
    internal class FakeAspUserManager(HashSet<User> users) : IAspUserManager
    {
        private readonly HashSet<User> _users = users;
        public IEnumerable<User> Users => _users;

        public Task<User?> FindByIdAsync(Guid id)
            => Task.FromResult(_users.SingleOrDefault(u => u.Id == id));

        public Task<IdentityResult> UpdateAsync(User user)
            => Task.FromResult(IdentityResult.Success);
    }
}
