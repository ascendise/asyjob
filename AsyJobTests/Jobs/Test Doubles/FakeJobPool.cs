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

        public void RunJob(Job job)
        {
            var jobThread = new Thread(job.Run);
            jobThread.Start();
            _jobThreads.Add(jobThread);
        }
    }
}
