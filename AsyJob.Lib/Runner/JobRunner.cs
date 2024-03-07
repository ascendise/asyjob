using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{

    public class JobRunner(IJobPool pool) : IJobRunner
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
