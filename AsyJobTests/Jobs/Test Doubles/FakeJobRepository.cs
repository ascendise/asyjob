using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class FakeJobRepository(List<Job> jobs) : IJobRepository
    {
        public IReadOnlyCollection<Job> Jobs { get => _jobs; }
        private readonly List<Job> _jobs = jobs;

        public FakeJobRepository() : this([]) { }

        public Task SaveJob(Job job)
        {
            this._jobs.Add(job);
            return Task.CompletedTask;
        }
    }
}
