using System.IO.MemoryMappedFiles;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AsyJob.Web.Auth
{
    /// <summary>
    /// This controller handles administrative operations for managing users
    /// </summary>
    /// <param name="usersApi"></param>
    /// <param name="userManager"></param>
    [Route("api/users")]
    [ApiController]
    [Produces("application/hal+json")]
    public class UsersController(
        IUsersApi usersApi, 
        IAspUserManager userManager,
        IMapper<User, UserResponse> userToUserResponseMapper,
        IMapper<UserResponse, HalUserResponse> userToHalMapper) : ControllerBase
    {
        private readonly IUsersApi _usersApi = usersApi;
        private readonly IAspUserManager _userManager = userManager;
        private readonly IMapper<User, UserResponse> _userToUserResponseMapper = userToUserResponseMapper;
        private readonly IMapper<UserResponse, HalUserResponse> _userToHalMapper = userToHalMapper;

        [HttpGet]
        [HasRights("Users_r")]
        public async Task<IEnumerable<HalUserResponse>> GetUsers()
        {
            var users = await _usersApi.GetAll();
            return users.Select(_userToHalMapper.Map);
        }


        [HttpPatch("{userId}")]
        [HasRights("Users_rw")]
        public async Task<HalUserResponse> Update(Guid userId, UserUpdateRequest request) 
        {
            var result = await _usersApi.Update(new(userId, request.Username, request.Rights));
            return _userToHalMapper.Map(result);
        }

        [HttpGet("/unconfirmed")]
        [HasRights("Users_r")]
        public Task<IEnumerable<HalUserResponse>> GetUnconfirmedUsers()
        {
            var users = _userManager.Users.Where(u => !u.ConfirmedByAdmin).ToList();
            var response = users.Select(_userToUserResponseMapper.Map)
                .Select(_userToHalMapper.Map);
            return Task.FromResult(response.AsEnumerable());
        }

        [HttpPost("/unconfirmed/{userId}/confirm")]
        [HasRights("Users_w")]
        public async Task<ActionResult> ConfirmUser(Guid userId, ConfirmUserRequest request) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
                return NotFound(); 
            user.ConfirmedByAdmin = true; 
            user.Rights = request.Rights.Select(r => new Right(r));
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
