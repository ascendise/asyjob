using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.HAL.AspNetCore;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Auth
{
    public class UserToUserResponseMapper() : IMapper<User, UserResponse>
    {
        public UserResponse Map(User src)
            => new(src.Id, src.UserName ?? src.Email ?? src.Id.ToString(), 
                src.Rights.Select(r => r.ToString()), src.ConfirmedByAdmin);
    }
}
