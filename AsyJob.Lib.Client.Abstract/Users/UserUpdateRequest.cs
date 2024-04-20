namespace AsyJob.Lib.Client.Abstract.Users
{
    public record UserUpdateRequest(string? Username = null, IEnumerable<string>? Rights = null);
}