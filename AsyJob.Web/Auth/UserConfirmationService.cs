using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web.Auth;

public class UserConfirmationService : IUserConfirmation<User>
{
    public Task<bool> IsConfirmedAsync(UserManager<User> _, User user)
        => Task.FromResult(user.ConfirmedByAdmin);
}
