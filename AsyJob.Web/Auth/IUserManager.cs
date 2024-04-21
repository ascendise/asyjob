namespace AsyJob.Web.Auth
{
    public interface IUserManager
    {
        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAll();
        /// <summary>
        /// Adds user to whitelist, so he can create an account  
        /// </summary>
        /// <param name="user"></param>
        void Whitelist(string email);
        /// <summary>
        /// Bans user. The user can no longer register or login with the provided email 
        /// </summary>
        /// <param name="userId"></param>
        void Ban(string email);
        /// <summary>
        /// Update user info and rights
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User Update(Guid userId, UpdateUserRequest user);
    }
}
