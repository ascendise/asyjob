using MongoDB.Driver;

namespace AsyJob.Jobs
{
    public interface IJobRepository
    {
        Task SaveJob(Job job);
        Task<IEnumerable<Job>> FetchAllJobs();
        Task<Job?> FetchJob(string id);
    }

    public class JobMongoRepository(IConfiguration config) : IJobRepository
    {
        private readonly IConfiguration _config = config;

        public async Task SaveJob(Job job)
        {
            var collection = GetJobCollection();
            await collection.InsertOneAsync(job);
        }

        private IMongoCollection<Job> GetJobCollection()
        {
            var connectionString = _config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            return client.GetDatabase("asyJob").GetCollection<Job>("jobs");
        }

        public async Task<IEnumerable<Job>> FetchAllJobs()
        {
            var collection = GetJobCollection();
            var cursor = await collection.FindAsync(_ => true);
            return await cursor.ToListAsync();
        }

        public async Task<Job?> FetchJob(string id)
        {
            var collection = GetJobCollection();
            var cursor = await collection.FindAsync(j => j.Id == id);
            return await cursor.FirstOrDefaultAsync();
        }
    }
}
