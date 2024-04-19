using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Web.HAL;
using AsyJob.Web.HAL.AspNetCore;
using Newtonsoft.Json;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Jobs
{
    public class HalJobResponse : HalDocument
    {
        [JsonExtensionData]
        public JobResponse JobResponse { get; private set; }

        public HalJobResponse(JobResponse response)
        {
            JobResponse = response; 
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
