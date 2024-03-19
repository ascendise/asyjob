namespace AsyJob.Lib.Auth
{
    public class User(Guid id, string username, IEnumerable<Right> rights)
    {
        public Guid Id { get; } = id;
        public string Username { get; } = username;
        public IEnumerable<Right> Rights { get; } = rights;
    }

}
