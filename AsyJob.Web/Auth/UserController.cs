using AsyJob.Web.Auth.Rights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [HasRights("Users_r")]
        public IEnumerable<HalUserResponse> GetUsers() => throw new NotImplementedException();

        [HttpPost]
        [HasRights("Users_w")]
        public ActionResult InviteUser(InviteUserRequest request) => throw new NotImplementedException();

        [HttpPost]
        public ActionResult BanUser(Guid userId) => throw new NotImplementedException();

        [HttpPatch]
        public HalUserResponse UpdateUser(UpdateUserRequest request) => throw new NotImplementedException();
    }
}
