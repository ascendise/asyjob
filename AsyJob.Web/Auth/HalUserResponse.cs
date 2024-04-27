using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.HAL;
using AsyJob.Web.HAL.AspNetCore;
using System.Security.Cryptography.Xml;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Auth
{
    public class HalUserResponse(Guid id, string username, IEnumerable<string> rights) : HalDocument
    {
        public HalUserResponse(User user) 
            : this(user.Id, 
                  user.UserName ?? user.Email ?? user.Id.ToString(),
                  user.Rights.Select(r => r.ToString()))
        {
            AddDefaultLinks();
        }

        public HalUserResponse(UserResponse user)
            : this(user.Id,
                  user.Username,
                  user.Rights)
        {
            AddDefaultLinks();
        }

        private void AddDefaultLinks()
        {
            Links.Add("users", LinkBuilder.New()
                .FromController(
                    typeof(UsersController),
                    nameof(UsersController.GetUsers))
                .Build());
            Links.Add("self", LinkBuilder.New()
                .FromController(
                    typeof(UsersController),
                    nameof(UsersController.Update),
                    new() { { "userId", Id } })
                .Build());
        }

        public Guid Id { get; private set; } = id;
        public string Username { get; private set; } = username;
        public IEnumerable<string> Rights { get; private set; } = rights;
    }
}