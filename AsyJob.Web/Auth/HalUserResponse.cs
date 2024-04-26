namespace AsyJob.Web.Auth
{
    public class HalUserResponse(Guid id, string username, IEnumerable<string> rights)
    {
        public HalUserResponse(User user) 
            : this(user.Id, 
                  user.UserName ?? user.Email ?? user.Id.ToString(),
                  user.Rights.Select(r => r.ToString()))
        {
        }


        public Guid Id { get; private set; } = id;
        public string Username { get; private set; } = username;
        public IEnumerable<string> Rights { get; private set; } = rights;
    }
}