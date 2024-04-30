﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Users
{
    internal class UserRegistrationTests
    {
        private readonly WebApplicationFactory<Program> _factory = new();

        [Test]
        public async Task RegisterUser_NotConfirmed_ShouldNotBeAbleToLogin()
        {
            //Arrange
            var sut = _factory.CreateClient();
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
    }
}
