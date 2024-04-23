using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Auth
{
    [Route("api/users")]
    [ApiController]
    [Produces("application/hal+json")]
    public class UsersController(IUsersApi usersApi, IUserStore<User> userStore) : ControllerBase
    {
        private readonly IUsersApi _usersApi = usersApi;
        private readonly IUserStore<User> _userStore = userStore;

        [HttpGet]
        [HasRights("Users_r")]
        public async Task<IEnumerable<HalUserResponse>> GetUsers()
        {
            var users = await _usersApi.GetAll();
            return users.Select(Map);
        }

        private HalUserResponse Map(UserResponse response)
            => new(response.Id, response.Username, response.Rights);


        [HttpPatch("{userId}")]
        [HasRights("Users_rw")]
        public Task<HalUserResponse> Update(Guid userId, UserUpdateRequest request) => throw new NotImplementedException();
    }
}
