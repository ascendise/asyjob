using AspNetCore.Identity.MongoDbCore.Models;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Runner;

namespace AsyJob.Web.Auth
{
    public class User : MongoIdentityUser<Guid>
    {
        public IEnumerable<Right> Rights { get; set; } = [];
        public bool ConfirmedByAdmin { get; set; } = false;

        public User(string username) : base(username) { }
        public User() : base() { }

        public Lib.Auth.User ToDomainUser()
        {
            return new(Id, UserName!, Rights)
            {
                Active = ConfirmedByAdmin
            };
        }
    }
}
