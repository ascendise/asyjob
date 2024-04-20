using AsyJob.Lib.Auth.Users;
using MongoDB.Driver;

namespace AsyJob.Web.Auth
{
    internal class MongoWhitelist(IConfiguration config) : IWhitelist
    {
        private readonly IConfiguration _config = config;

        public async Task Add(string email)
        {
            var whitelist = GetWhitelist();
            await whitelist.InsertOneAsync(new(email));
        }

        private IMongoCollection<WhitelistModel> GetWhitelist()
        {
            var client = new MongoClient();
            return client.GetDatabase("asyJob").GetCollection<WhitelistModel>("whitelist");
        }

        public async Task<IEnumerable<string>> GetWhitelistedEmails()
        {
            var whitelist = GetWhitelist();
            var documents = (await whitelist.FindAsync(_ => true)).ToEnumerable();
            return documents.Select(d => d.Email);
        }

        public async Task Remove(string email)
        {
            var whitelist = GetWhitelist();
            await whitelist.DeleteOneAsync(m => m.Email == email);
        }
    }
}
