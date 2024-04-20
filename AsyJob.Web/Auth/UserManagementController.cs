using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Auth
{
    /// <summary>
    /// Admin API for managing users
    /// </summary>
    [Route("api/users")]
    [ApiController]
    [Produces("application/hal+json")]
    public class UserManagementController : ControllerBase
    {

        [HttpGet]
        [HasRights("Users_r")]
        public IEnumerable<HalUserResponse> GetUsers() => throw new NotImplementedException();

        [HttpPost("/invite")]
        [HasRights("Users_w")]
        public ActionResult InviteUser(InviteUserRequest request) => throw new NotImplementedException();

        [HttpPost("/{userId}/ban")]
        public ActionResult BanUser(Guid userId) => throw new NotImplementedException();

        [HttpPatch("/{userId}")]
        public HalUserResponse UpdateUser(Guid userId, UpdateUserRequest request) => throw new NotImplementedException();
    }
}
