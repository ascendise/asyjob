using AsyJob.Lib.Auth.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    internal class FakeWhitelist : IWhitelist
    {
        public IEnumerable<string> AllowedEmails { get => _whitelist; }
        private readonly HashSet<string> _whitelist = [];
        public Task Add(string email)
        {
            _whitelist.Add(email);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetWhitelistedEmails()
            => Task.FromResult(_whitelist as IEnumerable<string>);

        public Task Remove(string email)
        {
            _whitelist.Remove(email);
            return Task.CompletedTask;
        }
    }
}
