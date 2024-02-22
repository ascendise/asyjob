namespace AsyJob.Jobs
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
    }

    public class JobPool(IJobRepository jobRepo) : IJobPool
    {
        private readonly IJobRepository _jobRepo = jobRepo;
        private readonly List<Thread> _jobThreads = [];

        public void RunJob(Job job)
        {
            _jobRepo.SaveJob(job);
            var thread = new Thread(job.Run);
            _jobThreads.Add(thread);
            thread.Start();
        }

        public async Task<T?> FetchJob<T>(string jobId) where T : Job
        {
            var job = await _jobRepo.FetchJob(jobId) as T;
            return job;
        }
    }
}
