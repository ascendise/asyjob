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
    internal class UserRegistrationTests
    {
        private readonly WebApplicationTestFactory<Program> _webAppFactory = new();

        [Test]
        public async Task RegisterUser_NotConfirmed_ShouldNotBeAbleToLogin()
        {
            //Arrange
            var sut = _webAppFactory.CreateClient();
            //Act
            await sut.PostAsJsonAsync("/register", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            var loginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "username@email.com",
                Password = "PyPassword-123"
            });
            //Assert
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task RegisterUser_ConfirmedByAdmin_ShouldBeAbleToLogin()
        {
            //Arrange
            var sut = _webAppFactory.CreateClient();
            //Act + Assert
            await sut.PostAsJsonAsync("/register", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            var adminLoginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "admin",
                Password = "HiMom-123"
            });
            Assert.That(adminLoginResponse.IsSuccessStatusCode, "Failed to login admin");
            var adminToken = await adminLoginResponse.ReadProperty(o => (string)o.accessToken);
            sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var unconfirmedUsersResponse = await sut.GetAsync("/api/users/unconfirmed");
            var registeredUser = JsonConvert.DeserializeObject<IEnumerable<UnconfirmedUserResponse>>(
                await unconfirmedUsersResponse.Content.ReadAsStringAsync())
                ?.Single();
            var confirmResponse = await sut.PostAsJsonAsync(registeredUser?.Links?.Confirm!.Href, new
            {
                Rights = Array.Empty<object>()
            });
            Assert.That(confirmResponse.IsSuccessStatusCode, "Failed to confirm user");
            var loginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
