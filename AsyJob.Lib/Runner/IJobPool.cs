using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{
    /// <summary>
    /// Interface for managing multiple running jobs.
    /// Classes that implement this interface must implement a way
    /// to not only run the jobs in a non-blocking way, but also persist the jobs
    /// between restarts.
    /// </summary>
    public interface IJobPool
    {
        /// <summary>
        /// Creates a new thread for running the job
        /// </summary>
        /// <param name="job"></param>
        void RunJob(Job job);

        /// <summary>
        /// Looks for a job with the specified id and returns it.
        /// If no job with this id exists, the returned value is null.
        /// </summary>
        /// <typeparam name="T">type of job with this Id</typeparam>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<T?> FetchJob<T>(string jobId) where T : Job;

        /// <summary>
        /// Returns all jobs in the pool
        /// </summary>
        /// <typeparam name="T">used to get list of specific job type</typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> FetchAll<T>() where T : Job;

        /// <summary>
        /// Returns all jobs in the pool
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Job>> FetchAll() => FetchAll<Job>();
    }
}