using AsyJob.Lib.Auth.Users;
using MongoDB.Driver;

namespace AsyJob.Web.Auth
{
    internal class MongoBans(IConfiguration config) : IBans
    {
        private readonly IConfiguration _config = config;

        public async Task Ban(string email)
        {
            var bans = GetBans();
            await bans.InsertOneAsync(new(email));
        }

        private IMongoCollection<BanModel> GetBans()
        {
            var conn = _config.GetConnectionString("MongoDB");
            var client = new MongoClient(conn);
            return client.GetDatabase("asyJob").GetCollection<BanModel>("bans");
        }

        public async Task<IEnumerable<string>> GetBannedEmails()
        {
            var bans = GetBans();
            var documents = (await bans.FindAsync(_ => true)).ToEnumerable();
            return documents.Select(d => d.Email);
        }

        public async Task Unban(string email)
        {
            var bans = GetBans();
            await bans.DeleteOneAsync(b => b.Email == email);
        }
    }
}
