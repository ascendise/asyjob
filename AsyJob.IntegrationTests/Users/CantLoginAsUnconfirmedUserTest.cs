using AsyJob.Web.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Users
{
    internal class CantLoginAsUnconfirmedUserTest : DefaultIntegrationTestHarness
    {
        private readonly WebApplicationTestFactory<Program> _webAppFactory = new();

        [Test]
        public async Task RegisterUser_NotConfirmed_ShouldNotBeAbleToLogin()
        {
            var sut = _webAppFactory.CreateClient();
            //Register new user
            await sut.PostAsJsonAsync("/register", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            //Try to login with new user without being confirmed
            var loginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "username@email.com",
                Password = "PyPassword-123"
            });
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
