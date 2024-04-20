using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Auth
{
    [Route("api/users")]
    [ApiController]
    [Produces("application/hal+json")]
    public class UsersController : ControllerBase
    {
        [HttpGet()]
        [HasRights("Users_r")]
        public Task<IEnumerable<HalUserResponse>> GetUsers() => throw new NotImplementedException();

        [HttpPost("/invite/")]
        [HasRights("Users_w")]
        public Task<ActionResult> Invite(InviteRequest request) => throw new NotImplementedException();

        [HttpPost("{userId}/ban")]
        [HasRights("Users_w")]
        public Task<ActionResult> Ban(Guid userId) => throw new NotImplementedException();

        [HttpPatch("{userId}")]
        [HasRights("Users_rw")]
        public Task<HalUserResponse> Update(Guid userId, UserUpdateRequest request) => throw new NotImplementedException();
    }
}
