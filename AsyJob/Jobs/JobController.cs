using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Jobs
{
    [Route("api/jobs")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        public Task<JobResponseDto> RunJob(JobRequestDto jobRequest)
        {
            return Task.FromResult((null as JobResponseDto)!);
        }
    }
}
