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
    public class JobController : ControllerBase
    {
        /// <summary>
        /// Creates a new job and runs it.
        /// </summary>
        /// <param name="jobRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<JobResponseDto> RunJob(JobRequestDto jobRequest)
        {
            return Task.FromResult((null as JobResponseDto)!);
        }
    }
}
