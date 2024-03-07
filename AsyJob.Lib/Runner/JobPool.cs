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

    public class JobPool : IJobPool
    {
        public int RunningThreads { get => _jobs.Select(p => p.Value.Thread).Where(v => v != null).Count(); }
        private readonly IJobRepository _jobRepo;
        private readonly Dictionary<string, JobThread> _jobs = [];

        private JobPool(IJobRepository jobRepo)
        {
            _jobRepo = jobRepo;
        }

        public static async Task<JobPool> StartJobPool(IJobRepository jobRepo)
        {
            var jobPool = new JobPool(jobRepo);
            foreach (var job in await jobRepo.FetchAllJobs())
            {
                jobPool.LoadJob(job);
            }
            return jobPool;
        }

        private void LoadJob(Job job)
        {
            if (job.Finished)
            {
                _jobs.Add(job.Id, new JobThread(job, null));
            }
            else
            {
                RunNewJobThread(job);
            }
        }

        private void RunNewJobThread(Job job)
        {
            var thread = new Thread(job.Run);
            _jobs.Add(job.Id, new JobThread(job, thread));
            thread.Start();
        }

        public void RunJob(Job job)
        {
            _jobRepo.SaveJob(job);
            RunNewJobThread(job);
        }

        public Task<T?> FetchJob<T>(string jobId) where T : Job
        {
            var job = _jobs.GetValueOrDefault(jobId);
            return Task.FromResult(job.Job as T);
        }

        public Task<IEnumerable<T>> FetchAll<T>() where T : Job
        {
            var jobs = _jobs.Values.Select(v => v.Job)
                .Where(j => j is T)
                .Select(j => (j as T)!);
            return Task.FromResult(jobs);
        }

        private readonly struct JobThread(Job job, Thread? thread)
        {
            public readonly Job Job = job;
            public readonly Thread? Thread = thread;
        }

    }
}