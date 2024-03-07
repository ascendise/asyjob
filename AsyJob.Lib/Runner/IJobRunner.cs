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
}
