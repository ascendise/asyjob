using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    internal class FakeUserRepository(HashSet<User>? users = null) : IUserRepository
    {
        public IEnumerable<User> Users { get => _users; }
        private readonly HashSet<User> _users = users ?? [];

        public Task<User?> Get(Guid id)
            => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

        public Task<IEnumerable<User>> GetAll()
            => Task.FromResult(_users as IEnumerable<User>);

        public Task Remove(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is not null)
                _users.Remove(user);
            return Task.CompletedTask;
        }

        public Task<User> Update(User user)
        {
            var oldUser = _users.FirstOrDefault(u => u.Id == user.Id)
                ?? throw new KeyNotFoundException($"No user with id {user.Id} found");
            _users.Remove(oldUser);
            _users.Add(user);
            return Task.FromResult(user);
        }
    }
}
