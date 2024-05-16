using AsyJob.IntegrationTests.TestHarness;
using AsyJob.IntegrationTests.Users;
using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Jobs;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Jobs
{
    internal class RunAJobAndPollForResponseTest : DefaultIntegrationTestHarness
    {
        private static readonly string[] s_jobRights = ["Jobs_rwx"];
        private readonly User User = new("user@mail.com", "HiMom-123", s_jobRights);

        private readonly SitemapSetUp _sitemapSetUp;
        private readonly RegisterNewUserSetUp _registerNewUserSetUp;

        private SitemapResponse Sitemap { get => _sitemapSetUp.Sitemap; }


        public RunAJobAndPollForResponseTest() : base()
        {
            _sitemapSetUp = new(Sut);
            _registerNewUserSetUp = new(User, Sut, Configuration, _sitemapSetUp);
            SetUps.AddRange([_sitemapSetUp, _registerNewUserSetUp]);
        }

        [Test]
        public async Task LoginAsUser_RunACounterJob_PollForTheResponseUntilItIsFinished()
        {
            //Login as user
            var loginResponse = await Sut.PostAsJsonAsync(Sitemap.Links.Login.Href, new
            {
                User.Email,
                User.Password,
            });
            Assert.That(loginResponse.IsSuccessStatusCode, "Failed to login as created user");
            Sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await loginResponse.ReadProperty<string>("accessToken"));
            //Run new job
            var jobResponse = await Sut.PostAsJsonAsync(Sitemap.Links.Jobs.Href, new JobRequest(
                jobtype: "CounterJob",
                name: "My Counter Job",
                description: "Test Counter Job",
                input: new Dictionary<string, object?>()
                {
                    { "DelayMs", 100},
                    { "Goal", 20 },
                    { "Increment", 1 }
                })
            );
            Assert.That(jobResponse.IsSuccessStatusCode, "Failed to run new job");
            var job = JsonConvert.DeserializeObject<JobResponse>(await jobResponse.Content.ReadAsStringAsync());
            Assert.That(job, Is.Not.Null, "Running job did not return the started job");
            //Wait for job to finish 
            while (job.ProgressStatus != "Done" && job.ProgressStatus != "Error")
            {
                await Task.Delay(100);
                var pollJobResponse = await Sut.GetAsync(job.Links.Self.Href);
                var s = pollJobResponse.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<JobResponse>(await pollJobResponse.Content.ReadAsStringAsync());
            }
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(job.Id, Is.Not.Null.And.Not.EqualTo(""));
                Assert.That(job.Name, Is.EqualTo("My Counter Job"));
                Assert.That(job.Description, Is.EqualTo("Test Counter Job"));
                Assert.That(job.Input!["DelayMs"], Is.EqualTo(100));
                Assert.That(job.Input!["Goal"], Is.EqualTo(20));
                Assert.That(job.Input!["Increment"], Is.EqualTo(1));
                Assert.That(job.Output!["Progress"], Is.EqualTo(20));
            });
        }
    }
}
