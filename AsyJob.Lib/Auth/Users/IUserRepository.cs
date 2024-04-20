namespace AsyJob.Lib.Auth.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> Get(Guid id);
        Task Remove(Guid id);
        Task Update(User user);
    }
}