using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Web.HAL.AspNetCore;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Auth
{
    public class UserResponseToHalMapper(User? currentUser) : IMapper<UserResponse, HalUserResponse>
    {

        public HalUserResponse Map(UserResponse user)
        {
            var response = new HalUserResponse(user);
            if (!user.Active && CanWrite(currentUser))
                response.Links.Add("confirm", LinkBuilder.New()
                    .FromController(
                        typeof(UsersController),
                        nameof(UsersController.ConfirmUser),
                        new() { { "userId", user.Id } })
                    .Build()
               );
            return response;
        }

        private static bool CanWrite(User? user)
        {
            if (user == null)
                return false;
            Right? userResource = user.Rights.SingleOrDefault(r => r.Resource == "Users");
            return userResource is not null
                && userResource.Value.Ops.HasFlag(Operation.Write);
        }
    }
}
