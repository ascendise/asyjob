using System.Collections.ObjectModel;

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

    public class JobPool : IJobPool
    {
        public IReadOnlyList<Thread> Threads { get => _jobThreads; }
        private readonly IJobRepository _jobRepo;
        private readonly List<Thread> _jobThreads = [];

        private JobPool(IJobRepository jobRepo)
        {
            _jobRepo = jobRepo;
        }

        public static async Task<JobPool> StartJobPool(IJobRepository jobRepo)
        {
            var jobPool = new JobPool(jobRepo);
            foreach(var job in await jobRepo.FetchAllJobs())
            {
                jobPool.RunNewJobThread(job);
            }
            return jobPool;
        }

        private void RunNewJobThread(Job job)
        {
            var thread = new Thread(job.Run);
            _jobThreads.Add(thread);
            thread.Start();
        }

        public void RunJob(Job job)
        {
            _jobRepo.SaveJob(job);
            RunNewJobThread(job);
        }

        public async Task<T?> FetchJob<T>(string jobId) where T : Job
        {
            var job = await _jobRepo.FetchJob(jobId) as T;
            return job;
        }
    }
}
