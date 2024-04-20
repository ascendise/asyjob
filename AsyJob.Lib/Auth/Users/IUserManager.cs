namespace AsyJob.Lib.Auth.Users
{
    public interface IUserManager
    {
        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAll();
        /// <summary>
        /// Adds user to whitelist, so he can create an account  
        /// </summary>
        /// <param name="user"></param>
        Task Whitelist(string email);
        /// <summary>
        /// Bans user. The user can no longer register or login with the provided email 
        /// </summary>
        /// <param name="userId"></param>
        Task Ban(string email);
        /// <summary>
        /// Update user info and rights
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> Update(Guid userId, UserUpdate user);
    }
}
