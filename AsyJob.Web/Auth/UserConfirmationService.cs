using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web.Auth;

/// <summary>
/// This implementation of user confirmation simply checks for a flag in the user object
/// </summary>
public class UserConfirmationService : IUserConfirmation<User>
{
    public Task<bool> IsConfirmedAsync(UserManager<User> _, User user)
        => Task.FromResult(user.ConfirmedByAdmin);
}
