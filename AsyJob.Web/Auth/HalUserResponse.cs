namespace AsyJob.Web.Auth
{
    public class HalUserResponse(User user)
    {
        public Guid Id { get; private set; } = user.Id;
        public string Username { get; private set; } = user.UserName ?? user.Email ?? user.Id.ToString(); //TODO: define what is actually set...
        public IEnumerable<string> Rights { get; private set; } = user.Rights.Select(r => r.ToString());
    }
}