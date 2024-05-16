using AsyJob.Web.Auth;
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

        //TODO: Add links dynamically based on rights
        public Sitemap()
        {
            Links.Add("docs", LinkBuilder.New("swagger/index.html")
                .SetType(MediaTypeNames.Text.Html)
                .Build());
            Links.Add("register", LinkBuilder.New("register")
                .Build());
            Links.Add("login", LinkBuilder.New("login")
                .Build());
            Links.Add("jobs", LinkBuilder.New()
                .FromController(typeof(JobController), nameof(JobController.RunJob))
                .Build());
            Links.Add("users", LinkBuilder.New()
                .FromController(typeof(UsersController), nameof(UsersController.GetUsers))
                .Build());
            Links.Add("unconfirmedUsers", LinkBuilder.New()
                .FromController(typeof(UsersController), nameof(UsersController.GetUnconfirmedUsers))
                .Build());
        }
    }
}
