using AspNetCore.Identity.MongoDbCore.Models;

namespace AsyJob.Auth
{
    public class Role : MongoIdentityRole<Guid>
    {
        public Role() : base() { }
        public Role(string username) : base(username) { }
        public Role(string username, Guid key) : base(username, key) { }
    }
}
