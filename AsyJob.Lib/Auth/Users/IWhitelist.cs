namespace AsyJob.Lib.Auth.Users
{
    public interface IWhitelist
    {
        Task Add(string email);
        Task Remove(string email);
        Task<IEnumerable<string>> GetWhitelistedEmails();
    }
}