using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{

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
            job.OnUpdate += OnJobUpdate;
            var thread = new Thread(async () =>
            {
                job.Run();
                await _jobRepo.UpdateJob(job);
            });
            _jobs.Add(job.Id, new JobThread(job, thread));
            thread.Start();
        }

        private async void OnJobUpdate(object sender, UpdateEventArgs e)
        {
            await _jobRepo.UpdateJob(e.Job);
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