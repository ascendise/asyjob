namespace AsyJob.Lib.Auth.Users
{
    public record UserUpdate(
        string? Username = null,
        IEnumerable<Right>? Rights = null
    );
}