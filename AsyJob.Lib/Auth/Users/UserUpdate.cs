namespace AsyJob.Lib.Auth.Users
{
    public record UserUpdate(
        string? Username, 
        IEnumerable<Right> Rights
    );
}