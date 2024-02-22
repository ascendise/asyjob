using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class FakeJobPool : IJobPool
    {
        public IReadOnlyList<Thread> JobThreads { get => _jobThreads; }
        private readonly List<Thread> _jobThreads = []; 
        private readonly List<Job> _jobs = [];

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
    }
}
