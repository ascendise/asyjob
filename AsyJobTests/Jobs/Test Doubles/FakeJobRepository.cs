﻿using AsyJob;
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
            if(_jobs.Any(j => j.Id == job.Id))
            {
                throw new DuplicateKeyException();
            }
            this._jobs.Add(job);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Job>> FetchAllJobs()
        {
            return Task.FromResult(_jobs as IEnumerable<Job>);
        }

        public Task<Job?> FetchJob(string id)
        {
            return Task.FromResult(_jobs.FirstOrDefault(j => j.Id == id));
        }
    }
}
