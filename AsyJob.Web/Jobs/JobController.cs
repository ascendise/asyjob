using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Web.Auth;
using AsyJob.Web.Auth.Rights;
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
    public class JobController(IJobApi jobApi) : ControllerBase
    {
        private readonly IJobApi _jobApi = jobApi;

        /// <summary>
        /// Creates a new job and runs it.
        /// </summary>
        /// <param name="jobRequest"></param>
        /// <exception cref="JobInputMismatchException">Thrown when input does not match the job</exception>
        /// <exception cref="NoMatchingJobFactoryException">Thrown when no job factory for <see cref="HalJobRequest.JobType"/> exists </exception>
        /// <returns></returns>
        [HttpPost]
        [HasRights($"{Resources.Jobs}_wx")]
        public async Task<HalJobResponse> RunJob(JobRequest jobRequest)
        {
            var response = await _jobApi.RunJob(jobRequest);
            return new(response);
        }

        [HttpGet("{jobId}")]
        [HasRights($"{Resources.Jobs}_r")]
        public async Task<HalJobResponse> FetchJob(string jobId)
        {
            var response = await _jobApi.FetchJob(jobId);
            return new(response);
        }
    }
}
