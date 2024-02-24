namespace AsyJob.Jobs
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
    }
}
