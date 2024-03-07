using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
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
}
