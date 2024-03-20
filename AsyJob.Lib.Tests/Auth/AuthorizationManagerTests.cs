using AsyJob.Lib.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.Auth
{
    internal class AuthorizationManagerTests
    {

        [Test]
        public void AuthenticationContext_HasRequiredRights_ShouldRunAction()
        {
            //Arrange
            var sut = new AuthorizationManager();
            var user = new User(Guid.NewGuid(), "User", [new Right("MyResource", Operation.Execute | Operation.Write)]);
            List<Right> requiredRights = [new Right("MyResource", Operation.Execute | Operation.Write)];
            //Act
            sut.AuthenticatedContext(() =>
            {
                Assert.Pass("Action was ran");
            }, user, requiredRights);
            //Assert
            Assert.Fail("Action was not ran");
        }

        [Test]
        public void AuthenticationContext_DoesNotHaveRights_ShouldThrowException()
        {
            //Arrange
            var sut = new AuthorizationManager();
            var user = new User(Guid.NewGuid(), "n00b", []);
            List<Right> requiredRights = [new Right("Resource", Operation.Read)];
            //Act
            void fn() => sut.AuthenticatedContext(() =>
            {
                Assert.Fail("Action was run even tho user is not permitted to!");
            }, user, requiredRights);
            //Assert
            Assert.Throws<UnauthorizedException>(fn);
        }

        [Test]
        public void AuthenticationContext_DifferentRights_ShouldIncludeMissingRightsInException()
        {
            //Arrange
            var sut = new AuthorizationManager();
            var user = new User(Guid.NewGuid(), "HelloWorld", [new Right("Resource", Operation.Execute)]);
            List<Right> requiredRights = [new Right("Resource", Operation.Read | Operation.Write)];
            //Act
            void fn() => sut.AuthenticatedContext(() =>
            {
                Assert.Fail("Action was run even tho user is not permitted to!");
            }, user, requiredRights);
            //Assert
            var exception = Assert.Throws<UnauthorizedException>(fn);
            Assert.That(exception.MissingRights.Count, Is.EqualTo(1));
            Assert.That(exception.MissingRights.Single().Ops, Is.EqualTo(Operation.Read | Operation.Write));
        }


    }
}
