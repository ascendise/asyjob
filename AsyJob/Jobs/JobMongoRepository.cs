using MongoDB.Driver;

namespace AsyJob.Jobs
{
    public interface IJobRepository
    {
        /// <summary>
        /// Stores the job inside the repository
        /// </summary>
        /// <param name="job"></param>
        /// <exception cref="DuplicateKeyException">thrown when a job with the chosen id already exists</exception>
        /// <returns></returns>
        Task SaveJob(Job job);
        /// <summary>
        /// Returns all jobs inside the repository
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Job>> FetchAllJobs();
        /// <summary>
        /// Returns job with matching id or null, if no job was found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Job?> FetchJob(string id);
    }

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
            catch(MongoDuplicateKeyException)
            {
                throw new DuplicateKeyException($"A job with id {job.Id} already exists!");
            }
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
