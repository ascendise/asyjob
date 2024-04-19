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
        private readonly IMapper<JobRequest, Job> _domainMapper = new JobMapper(jobFactory);
        private readonly IMapper<Job, JobResponse> _responseMapper = new JobMapper(jobFactory);
        private readonly IJobRunner _jobRunner = jobRunner;

        public Task<JobResponse> RunJob(JobRequest jobReq)
        {
            var job = _domainMapper.Map(jobReq);
            _jobRunner.RunJob(job);
            var response = _responseMapper.Map(job);
            return Task.FromResult(response);
        }

        public async Task<IEnumerable<JobResponse>> FetchAllJobs()
        {
            var jobs = await _jobRunner.GetJobs();
            return jobs.Select(_responseMapper.Map);
        }

        public async Task<JobResponse> FetchJob(string jobId)
        {
            var job = await _jobRunner.GetJob(jobId);
            return job != null ? _responseMapper.Map(job) : throw new KeyNotFoundException();
        }
    }
}
