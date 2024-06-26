﻿using AsyJob.IntegrationTests.TestHarness;
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
    internal class CanLoginAsConfirmedUserTest : DefaultIntegrationTestHarness
    {
        private readonly WebApplicationTestFactory<Program> _webAppFactory = new();

        [Test]
        public async Task UserRegisters_IsConfirmedByAdmin_ShouldBeAbleToLogin()
        {
            var sut = _webAppFactory.CreateClient();
            //Register new user
            await sut.PostAsJsonAsync("/register", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            //Login as admin
            var adminLoginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "admin",
                Password = "HiMom-123"
            });
            Assert.That(adminLoginResponse.IsSuccessStatusCode, "Failed to login admin");
            var adminToken = await adminLoginResponse.ReadProperty<string>("accessToken");
            sut.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            //Fetch unconfirmed user
            //expecting it to include our newly registered User
            var unconfirmedUsersResponse = await sut.GetAsync("/api/users/unconfirmed");
            var registeredUser = JsonConvert.DeserializeObject<IEnumerable<UnconfirmedUserResponse>>(
                await unconfirmedUsersResponse.Content.ReadAsStringAsync())
                ?.Single();
            //Confirm user through link
            var confirmResponse = await sut.PostAsJsonAsync(registeredUser?.Links?.Confirm!.Href, new
            {
                Rights = Array.Empty<object>()
            });
            Assert.That(confirmResponse.IsSuccessStatusCode, "Failed to confirm user");
            //Try login as new user
            var loginResponse = await sut.PostAsJsonAsync("/login", new
            {
                Email = "username@email.com",
                Password = "MyPassword-123"
            });
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
