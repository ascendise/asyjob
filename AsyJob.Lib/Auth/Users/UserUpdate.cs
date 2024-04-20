namespace AsyJob.Lib.Auth.Users
{
    public record UserUpdate(
        string? Email, 
        string? Username, 
        IEnumerable<Right> Rights
    );
}