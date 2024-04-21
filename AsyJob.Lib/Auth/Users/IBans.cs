namespace AsyJob.Lib.Auth.Users
{
    public interface IBans
    {
        Task<IEnumerable<string>> GetBannedEmails();
        Task Ban(string email);
        Task Unban(string email);
    }
}