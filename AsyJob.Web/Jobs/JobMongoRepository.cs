using AsyJob.Lib;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Runner;
using MongoDB.Driver;

namespace AsyJob.Web.Jobs
{
    /// <summary>
    /// Repository for persisting jobs using the MongoDB.Driver
    /// </summary>
    /// <param name="config"></param>
    public class JobMongoRepository(IConfiguration config) : IJobRepository
    {
        private readonly IConfiguration _config = config;

        public async Task SaveJob(Job job)
        {
            var collection = GetJobCollection();
            try
            {
                await collection.InsertOneAsync(job);
            }
            catch (MongoDuplicateKeyException)
            {
                throw new DuplicateKeyException($"A job with id {job.Id} already exists!");
            }
        }

        private IMongoCollection<Job> GetJobCollection()
        {
            var connectionString = _config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            return client.GetDatabase(_config["DatabaseName"] ?? "asyJob").GetCollection<Job>("jobs");
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

        public async Task<Job> UpdateJob(Job job)
        {
            var collection = GetJobCollection();
            var filter = Builders<Job>.Filter
                .Eq(j => j.Id, job.Id);
            return await collection.FindOneAndReplaceAsync(filter, job);
        }

        public async Task DeleteJob(string jobId)
        {
            var collection = GetJobCollection();
            await collection.DeleteOneAsync(j => j.Id == jobId);
        }
    }
}
