using AsyJob.Lib.Auth;
using AsyJob.Web.Auth;
using Microsoft.AspNetCore.Identity;

namespace AsyJob.Web
{
    /// <summary>
    /// Set of extensions for setting up the application
    /// </summary>
    public static class BootstrapExtensions
    {
        public static async Task AddAdminUser(this IApplicationBuilder appBuilder)
        {
            using var scope = appBuilder.ApplicationServices.CreateScope();
            var sp = scope.ServiceProvider;
            var config = sp.GetRequiredService<IConfiguration>();
            var adminConfig = new AdminConfig();
            config.GetSection("Admin").Bind(adminConfig);
            ThrowIfMissingValues(adminConfig);
            var userManager = sp.GetRequiredService<UserManager<Auth.User>>();
            if (userManager.Users.Any(u => u.UserName == adminConfig.Username!))
                return; //Admin user already exists
            var user = new Auth.User()
            {
                UserName = adminConfig.Username,
                Email = adminConfig.Email,
                ConfirmedByAdmin = true,
                Rights = [
                    new Right(Resources.Jobs, Operation.Read | Operation.Write | Operation.Execute),
                    new Right("Users", Operation.Read | Operation.Write)
                ]
            };
            await userManager.CreateAsync(user, adminConfig.Password!);
        }

        private static void ThrowIfMissingValues(AdminConfig adminConfig)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(adminConfig.Username);
            ArgumentException.ThrowIfNullOrWhiteSpace(adminConfig.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(adminConfig.Password);
        }
    }
}
