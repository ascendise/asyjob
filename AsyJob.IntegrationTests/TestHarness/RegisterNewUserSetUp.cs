using AsyJob.IntegrationTests.Users;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.TestHarness
{
    /// <summary>
    /// Registers a new, confirmed user
    /// </summary>
    /// <param name="sut"></param>
    /// <param name="config"></param>
    internal class RegisterNewUserSetUp(User newUser, HttpClient sut, IConfiguration config, SitemapSetUp sitemapSetUp) : ISetUp
    {
        /// <summary>
        /// Newly registered user
        /// </summary>
        public User User { get; private set; } = newUser;

        private readonly HttpClient _sut = sut;
        private readonly IConfiguration _config = config;
        private readonly SitemapSetUp _sitemapSetUp = sitemapSetUp;

        public async Task SetUp()
        {
            var sitemap = _sitemapSetUp.Sitemap;
            await RegisterUser(sitemap);
            var accessToken = await GetAdminToken(sitemap);
            _sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await ConfirmUser(sitemap);
        }

        private async Task RegisterUser(SitemapResponse sitemap)
        {
            var registerResponse = await _sut.PostAsJsonAsync(sitemap.Links.Register.Href, new
            {
                User.Email,
                User.Password
            });
            Assert.That(registerResponse.IsSuccessStatusCode, "Failed to register new user");
        }

        private async Task<string> GetAdminToken(SitemapResponse sitemap)
        {
            var adminConfig = _config.GetSection("Admin");
            var adminLoginResponse = await _sut.PostAsJsonAsync(sitemap.Links.Login.Href, new
            {
                Email = adminConfig["Username"],
                Password = adminConfig["Password"]
            });
            Assert.That(adminLoginResponse.IsSuccessStatusCode, "Failed to login as admin for user confirmation");
            var accessToken = await adminLoginResponse.ReadProperty<string>("accessToken");
            Assert.That(accessToken, Is.Not.Null, "Access token not present in admin login response");
            return accessToken;
        }

        private async Task ConfirmUser(SitemapResponse sitemap)
        {
            UnconfirmedUserResponse unconfirmedUser = await FetchUnconfirmedUser(sitemap);
            await ConfirmUser(unconfirmedUser);
        }

        private async Task<UnconfirmedUserResponse> FetchUnconfirmedUser(SitemapResponse sitemap)
        {
            var unconfirmedUsersResponse = await _sut.GetAsync(sitemap.Links.UnconfirmedUsers.Href);
            Assert.That(unconfirmedUsersResponse.IsSuccessStatusCode, "Failed to confirm user");
            var unconfirmedUsers = JsonConvert.DeserializeObject<IEnumerable<UnconfirmedUserResponse>>(await unconfirmedUsersResponse.Content.ReadAsStringAsync());
            Assert.That(unconfirmedUsers, Is.Not.Null, "No unconfirmed users found");
            var unconfirmedUser = unconfirmedUsers.Single();
            return unconfirmedUser;
        }

        private async Task ConfirmUser(UnconfirmedUserResponse unconfirmedUser)
        {
            var confirmResponse = await _sut.PostAsJsonAsync(unconfirmedUser.Links.Confirm.Href, new
            {
                User.Rights
            });
            Assert.That(confirmResponse.IsSuccessStatusCode, "New user was not confirmed");
        }
    }
}
