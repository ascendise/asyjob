namespace AsyJob.Lib.Client.Abstract.Users
{
    public record UserUpdateRequest(Guid Id, string? Username = null, IEnumerable<string>? Rights = null);
}