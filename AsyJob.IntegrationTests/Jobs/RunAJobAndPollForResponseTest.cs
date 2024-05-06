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

        private HttpClient _sut = null!;
        private SitemapResponse _sitemap = null!;

        /// <summary>
        /// Register and confirm new user for running the test
        /// </summary>
        /// <returns></returns>
        public override async Task SetUp()
        {
            await base.SetUp();
            _sut = new WebApplicationTestFactory<Program>().CreateClient();
            var sitemapResponse = await _sut.GetAsync("api");
            Assert.That(sitemapResponse.IsSuccessStatusCode, Is.True, "Failed to fetch Sitemap");
            var sitemap = JsonConvert.DeserializeObject<SitemapResponse>(await sitemapResponse.Content.ReadAsStringAsync());
            Assert.That(sitemap, Is.Not.Null, "Sitemap response is empty");
            _sitemap = sitemap!;
            //Register user
            var registerResponse = await _sut.PostAsJsonAsync(_sitemap.Links.Register.Href, new
            {
                Email = "user@mail.com",
                Password = "HiMom-123"
            });
            Assert.That(registerResponse.IsSuccessStatusCode, "Failed to register new user");
            //Login as admin
            var adminConfig = Configuration.GetSection("Admin");
            var adminLoginResponse = await _sut.PostAsJsonAsync(_sitemap.Links.Login.Href, new
            {
                Email = adminConfig["Username"],
                Password = adminConfig["Password"]
            });
            Assert.That(adminLoginResponse.IsSuccessStatusCode, "Failed to login as admin for user confirmation");
            var accessToken = await adminLoginResponse.ReadProperty<string>("accessToken");
            _sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //Get unconfirmed user
            var unconfirmedUsersResponse = await _sut.GetAsync(_sitemap.Links.UnconfirmedUsers.Href);
            Assert.That(unconfirmedUsersResponse.IsSuccessStatusCode, "Failed to confirm user");
            var unconfirmedUsers = JsonConvert.DeserializeObject<IEnumerable<UnconfirmedUserResponse>>(await unconfirmedUsersResponse.Content.ReadAsStringAsync());
            Assert.That(unconfirmedUsers, Is.Not.Null, "No unconfirmed users found");
            var unconfirmedUser = unconfirmedUsers.Single();
            //Confirm user
            var confirmResponse = await _sut.PostAsJsonAsync(unconfirmedUser.Links.Confirm.Href, new
            {
                Rights = s_jobRights
            });
            Assert.That(confirmResponse.IsSuccessStatusCode, "New user was not confirmed");
        }

        [Test]
        public async Task LoginAsUser_RunACounterJob_PollForTheResponseUntilItIsFinished()
        {
            //Login as user
            var loginResponse = await _sut.PostAsJsonAsync(_sitemap.Links.Login.Href, new
            {
                Email = "user@mail.com",
                Password = "HiMom-123"
            });
            Assert.That(loginResponse.IsSuccessStatusCode, "Failed to login as created user");
            _sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await loginResponse.ReadProperty<string>("accessToken"));
            //Run new job
            var jobResponse = await _sut.PostAsJsonAsync(_sitemap.Links.Jobs.Href, new JobRequest(
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
                var pollJobResponse = await _sut.GetAsync(job.Links.Self.Href);
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
