using System.IO.MemoryMappedFiles;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Auth
{
    [Route("api/users")]
    [ApiController]
    [Produces("application/hal+json")]
    public class UsersController(IUsersApi usersApi, UserManager<User> userManager) : ControllerBase
    {
        private readonly IUsersApi _usersApi = usersApi;
        private readonly UserManager<User> _userManager = userManager;

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
        public async Task<HalUserResponse> Update(Guid userId, UserUpdateRequest request) 
        {
            var result = await _usersApi.Update(new(request.Id, request.Username, request.Rights));
            return Map(result);
        }

        [HttpGet("/unconfirmed")]
        [HasRights("Users_r")]
        public Task<IEnumerable<HalUserResponse>> GetUnconfirmedUsers()
        {
            var users = _userManager.Users.Where(u => !u.ConfirmedByAdmin).ToList();
            var response = users.Select(u => new HalUserResponse(u.Id, u.UserName ?? u.Email ?? u.Id.ToString(), u.Rights.Select(r => r.ToString())));
            return Task.FromResult(response.AsEnumerable());
        }

        [HttpPost("/unconfirmed/{userId}/confirm")]
        [HasRights("Users_w")]
        public async Task<ActionResult> ConfirmUser(Guid userId, ConfirmUserRequest request) 
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user is null)
                return NotFound(); 
            user.ConfirmedByAdmin = true; 
            user.Rights = request.Rights.Select(r => new Right(r));
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
