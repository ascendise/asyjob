using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Web.HAL;
using AsyJob.Web.HAL.AspNetCore;
using Newtonsoft.Json.Linq;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Jobs
{
    public class HalJobResponse : HalDocument
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ProgressStatus { get; private set; }
        public IDictionary<string, object?>? Input { get; private set; }
        public IDictionary<string, object?>? Output { get; private set; }
        public Exception? Error { get; private set; }

        public HalJobResponse(JobResponse response)
        {
            Id = response.Id;
            Name = response.Name;
            Description = response.Description;
            ProgressStatus = response.ProgressStatus;
            Input = response.Input;
            Output = response.Output;
            Error = response.Error;
            AddDefaultLinks();
        }

        private static Dictionary<string, JToken?>? ToJsonExtension(IDictionary<string, object?>? output)
            => output?.Select(kp =>
            {
                JToken? token = null;
                if (kp.Value is not null)
                {
                    token = JToken.FromObject(kp.Value);
                }
                return KeyValuePair.Create(kp.Key, token);
            })
            .ToDictionary();

        private void AddDefaultLinks()
        {
            Links.Add("jobs", LinkBuilder.New()
                .FromController(typeof(JobController), nameof(JobController.FetchJob))
                .Build());
        }
    }
}
