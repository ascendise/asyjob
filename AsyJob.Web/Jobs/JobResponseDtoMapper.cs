using AsyJob.Lib.Jobs;
using AsyJob.Web.HAL.AspNetCore;
using AsyJob.Web.Mapping;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Jobs
{
    public class JobResponseDtoMapper : IMapper<Job, JobResponseDto>
    {
        public JobResponseDto Map(Job job)
        {
            var dto = new JobResponseDto(job);
            dto.Links.Add("self", LinkBuilder.New()
                .FromController(typeof(JobController), nameof(JobController.FetchJob), new() { { "jobId", job.Id} })
                .Build()
            );
            return dto;
        }
    }
}
