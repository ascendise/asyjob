using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Client.Jobs
{
    public class JobApi(JobFactory jobFactory, IJobRunner jobRunner) : IJobApi
    {
        private readonly JobMapper _jobMapper = new(jobFactory);
        private readonly IJobRunner _jobRunner = jobRunner;

        public Task<JobResponse> RunJob(JobRequest jobReq)
        {
            var job = _jobMapper.Map(jobReq);
            _jobRunner.RunJob(job);
            var response = _jobMapper.Map(job);
            return Task.FromResult(response);
        }

        public async Task<IEnumerable<JobResponse>> FetchAllJobs()
        {
            var jobs = await _jobRunner.GetJobs();
            return jobs.Select(_jobMapper.Map);
        }

        public async Task<JobResponse> FetchJob(string jobId)
        {
            var job = await _jobRunner.GetJob(jobId);
            return job != null ? _jobMapper.Map(job) : throw new KeyNotFoundException();
        }
    }
}
