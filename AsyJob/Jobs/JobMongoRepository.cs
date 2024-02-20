using MongoDB.Driver;

namespace AsyJob.Jobs
{
    public interface IJobRepository
    {
        Task SaveJob(Job job);
    }

    public class JobMongoRepository(IConfiguration config) : IJobRepository
    {
        private readonly IConfiguration _config = config;
        public async Task SaveJob(Job job)
        {
            var connectionString = _config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var collection = client.GetDatabase("asyJob").GetCollection<Job>("jobs");
            await collection.InsertOneAsync(job);
        }
    }
}
