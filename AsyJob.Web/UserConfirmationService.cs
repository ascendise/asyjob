using AsyJob.Web.Auth;
using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web;

public class UserConfirmationService : IUserConfirmation<User>
{
    public Task<bool> IsConfirmedAsync(UserManager<User> _, User user) 
        => Task.FromResult(user.ConfirmedByAdmin);
}
