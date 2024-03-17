using AspNetCore.Identity.MongoDbCore.Models;

namespace AsyJob.Auth
{
    public class User : MongoIdentityUser<Guid>
    {
        public User(string username): base(username) { }
        public User() : base() { }
    }
}
