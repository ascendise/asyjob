using AsyJob.Lib.Auth;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using AsyJob.Web.Auth;
using AsyJob.Web.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web.Jobs
{
    /// <summary>
    /// Controller for Job API
    /// Used to run new jobs and fetch the state of created ones
    /// </summary>
    [Route("api/jobs")]
    [ApiController]
    [Produces("application/hal+json")]
    public class JobController(IJobRunner jobRunner, JobFactory jobFactory, IMapper<Job, JobResponseDto> mapper) : ControllerBase
    {
        private readonly IJobRunner _jobRunner = jobRunner;
        private readonly JobFactory _jobFactory = jobFactory;
        private readonly IMapper<Job, JobResponseDto> _mapper = mapper;

        /// <summary>
        /// Creates a new job and runs it.
        /// </summary>
        /// <param name="jobRequest"></param>
        /// <exception cref="JobInputMismatchException">Thrown when input does not match the job</exception>
        /// <exception cref="NoMatchingJobFactoryException">Thrown when no job factory for <see cref="JobRequestDto.JobType"/> exists </exception>
        /// <returns></returns>
        [HttpPost]
        [HasRights($"{nameof(JobRunner)}_wx")]
        public Task<JobResponseDto> RunJob(JobRequestDto jobRequest)
        {
            Job job = CreateJob(jobRequest);
            _jobRunner.RunJob(job);
            return Task.FromResult(_mapper.Map(job));
        }

        private Job CreateJob(JobRequestDto jobRequest)
        {
            if (jobRequest.Input == null)
            {
                return _jobFactory.CreateJob(jobRequest.JobType, jobRequest.Name, jobRequest.Description);
            }
            return _jobFactory.CreateJobWithInput(jobRequest.JobType, jobRequest.Input, jobRequest.Name, jobRequest.Description);
        }

        [HttpGet("{jobId}")]
        [HasRights($"{nameof(JobRunner)}_r")]
        public async Task<JobResponseDto> FetchJob(string jobId)
        {
            var job = await _jobRunner.GetJob(jobId);
            return job != null ? _mapper.Map(job) : throw new KeyNotFoundException();
        }
    }
}
