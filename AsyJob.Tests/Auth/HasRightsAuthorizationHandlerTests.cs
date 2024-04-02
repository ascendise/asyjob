using AsyJob.Auth;
using AsyJob.Lib.Auth;
using AsyJob.Tests.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User = AsyJob.Auth.User;

namespace AsyJob.Tests.Auth
{
    internal class HasRightsAuthorizationHandlerTests
    {

        [Test]
        public async Task HandleRequirementAsync_HasRights_ShouldSucceed()
        {
            //Arrange
            var id = Guid.Parse("bd5a92fc-b4bd-4b8e-a0d1-c1ff8f56c717");
            var mongoUser = new User()
            {
                Id = id,
                Rights = [
                    new("Resource", Operation.Write | Operation.Execute)
                ]
            };
            var identityPrincipal = CreatePrincipal(id.ToString());
            var fakeUserStore = new FakeUserStore([mongoUser]);
            var sut = new HasRightsAuthorizationHandler(fakeUserStore);
            var requirement = new HasRightsRequirement([new("Resource", Operation.Write | Operation.Execute)]);
            var authContext = new AuthorizationHandlerContext([requirement], identityPrincipal, null);
            //Act
            await sut.HandleAsync(authContext);
            //Assert
            Assert.That(authContext.HasSucceeded);
        }

        private static ClaimsPrincipal CreatePrincipal(string id)
        {
            var identity = new ClaimsIdentity(null, new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id)
            });
            return new ClaimsPrincipal(identity);
        }

        [Test]
        public async Task HandleRequirementAsync_DoesNotHaveRights_ShouldFail()
        {
            //Arrange
            var id = Guid.Parse("bd5a92fc-b4bd-4b8e-a0d1-c1ff8f56c717");
            var mongoUser = new User()
            {
                Id = id,
                Rights = [
                    new("Resource", Operation.Read)
                ]
            };
            var fakeUserStore = new FakeUserStore([mongoUser]);
            var sut = new HasRightsAuthorizationHandler(fakeUserStore);
            var requirement = new HasRightsRequirement([new("Resource", Operation.Write | Operation.Execute)]);
            var identityPrincipal = CreatePrincipal(id.ToString());
            var authContext = new AuthorizationHandlerContext([requirement], identityPrincipal, null);
            //Act
            await sut.HandleAsync(authContext);
            //Assert
            Assert.That(authContext.HasFailed);
        }

    }
}
