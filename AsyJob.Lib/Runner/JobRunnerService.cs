using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{
    public interface IJobRunner
    {
        /// <summary>
        /// Runs the job. May persist
        /// </summary>
        /// <param name="job"></param>
        void RunJob(Job job);

        /// <summary>
        /// Returns all jobs in pool
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Job>> GetJobs();

        /// <summary>
        /// Returns job with matching id or null
        /// </summary>
        Task<Job?> GetJob(string jobId);
    }

    public class JobRunnerService(IJobPool pool) : IJobRunner
    {
        private readonly IJobPool _pool = pool;

        public void RunJob(Job job)
        {
            _pool.RunJob(job);
        }

        public Task<IEnumerable<Job>> GetJobs()
        {
            return _pool.FetchAll<Job>();
        }

        public Task<Job?> GetJob(string jobId)
        {
            return _pool.FetchJob<Job>(jobId);
        }
    }
}
