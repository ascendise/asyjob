using AsyJob.Lib.Jobs;
using AsyJob.Web.HAL;
using AsyJob.Web.HAL.AspNetCore;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Jobs
{

    public class JobResponseDto : HalDocument
    {
        public Job Job { get; private set; }

        public JobResponseDto(Job job)
        {
            Job = job;
            AddDefaultLinks();
        }

        private void AddDefaultLinks()
        {
            Links.Add("jobs", LinkBuilder.New()
                .FromController(typeof(JobController), nameof(JobController.FetchJob))
                .Build());
        }
    }
}
