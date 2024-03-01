using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Jobs
{
    /// <summary>
    /// Controller for Job API
    /// Used to run new jobs and fetch the state of created ones
    /// </summary>
    [Route("api/jobs")]
    [ApiController]
    public class JobController(IJobRunner jobRunner, JobFactory jobFactory) : ControllerBase
    {
        private readonly IJobRunner _jobRunner = jobRunner;
        private readonly JobFactory _jobFactory = jobFactory;

        /// <summary>
        /// Creates a new job and runs it.
        /// </summary>
        /// <param name="jobRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<JobResponseDto> RunJob(JobRequestDto jobRequest)
        {
            Job job = CreateJob(jobRequest);
            _jobRunner.RunJob(job);
            return Task.FromResult(new JobResponseDto(job));
        }

        private Job CreateJob(JobRequestDto jobRequest)
        {
            if (jobRequest.Input == null)
            {
                return _jobFactory.CreateJob(jobRequest.JobType, jobRequest.Name, jobRequest.Description);
            }
            return _jobFactory.CreateJob<object>(jobRequest.JobType, jobRequest.Name, jobRequest.Description);
        }
    }
}
