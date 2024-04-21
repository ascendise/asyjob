using AsyJob.Lib.Auth.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    internal class FakeBans(HashSet<string>? initBans = null) : IBans
    {
        public IEnumerable<string> BannedEmails { get => _bannedEmails; }
        private readonly HashSet<string> _bannedEmails = initBans ?? [];

        public Task Ban(string email)
        {
            _bannedEmails.Add(email);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetBannedEmails()
            => Task.FromResult(_bannedEmails as IEnumerable<string>);

        public Task Unban(string email)
        {
            _bannedEmails.Remove(email);
            return Task.CompletedTask;
        }
    }
}
