using AsyJob.Web.HAL;
using AsyJob.Web.HAL.AspNetCore;
using AsyJob.Web.Jobs;
using System.Net.Mime;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web
{
    public class Sitemap : HalDocument
    {
        public string Message { get => "Welcome to the Asynchronous Job API! Thanks for being here!"; }

        public Sitemap()
        {
            Links.Add("jobs", LinkBuilder.New()
                .FromController(typeof(JobController), nameof(JobController.FetchJob))
                .Build());
            Links.Add("docs", LinkBuilder.New("swagger/index.html")
                .SetType(MediaTypeNames.Text.Html)
                .Build());
        }
    }
}
