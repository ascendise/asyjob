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
    }

    public class JobPool(IJobRepository jobRepo) : IJobPool
    {
        private readonly IJobRepository _jobRepo = jobRepo;
        private readonly ICollection<Thread> _jobThreads = new List<Thread>();

        public void RunJob(Job job)
        {
            _jobRepo.SaveJob(job);
            var thread = new Thread(job.Run);
            _jobThreads.Add(thread);
            thread.Start();
        }
    }
}
