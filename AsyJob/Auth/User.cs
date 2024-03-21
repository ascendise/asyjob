using AspNetCore.Identity.MongoDbCore.Models;
using AsyJob.Lib.Auth;

namespace AsyJob.Auth
{
    public class User : MongoIdentityUser<Guid>
    {
        public User(string username) : base(username) { }
        public User() : base() { }

        public Lib.Auth.User GetDomainUser()
        {
            var claimToRightConverter = new ClaimToRightConverter();
            var rights = Claims.Select(claimToRightConverter.ToRight);
            return new(Id, UserName!, rights);
        }
    }
}
