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
        /// Updates the user
        /// </summary>
        /// <param name="user"></param>
        Task<User> Update(Guid userId, UserUpdate user);
    }
}
