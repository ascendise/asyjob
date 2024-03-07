using AsyJob.Lib.Jobs;
using AsyJob.Lib.Runner;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class FakeJobPool : IJobPool
    {
        public IReadOnlyList<Thread> JobThreads { get => _jobThreads; }
        private readonly List<Thread> _jobThreads = [];
        private readonly List<Job> _jobs = [];

        public static FakeJobPool InitializePool(IEnumerable<Job> initJobs)
        {
            var pool = new FakeJobPool();
            foreach (var job in initJobs)
            {
                pool.RunJob(job);
            }
            return pool;
        }

        public void RunJob(Job job)
        {
            var jobThread = new Thread(job.Run);
            jobThread.Start();
            _jobThreads.Add(jobThread);
            _jobs.Add(job);
        }

        public Task<T?> FetchJob<T>(string jobId) where T : Job
        {
            var job = _jobs.FirstOrDefault(j => j.Id == jobId) as T;
            return Task.FromResult(job);
        }

        public Task<IEnumerable<T>> FetchAll<T>() where T : Job
        {
            var jobs = _jobs.Where(j => j is T)
                .Select(j => (j as T)!);
            return Task.FromResult(jobs);
        }
    }
}
