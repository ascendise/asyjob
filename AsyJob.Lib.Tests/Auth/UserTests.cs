using AsyJob.Lib.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.Auth
{
    internal class UserTests
    {

        [Test]
        public void Needs_HasMissingRights_ShouldReturnMissingRightsOnly()
        {
            //Arrange
            var user = new User(Guid.NewGuid(), "Username", [new Right("Resource", Operation.Write)]);
            List<Right> requiredRights = [new Right("Resource", Operation.Execute)];
            //Act
            var missingRights = user.Needs(requiredRights);
            //Assert
            Assert.That(missingRights.Count(), Is.EqualTo(1));
            var missingRight = missingRights.Single();
            Assert.Multiple(() =>
            {
                Assert.That(missingRight.Resource, Is.EqualTo("Resource"));
                Assert.That(missingRight.Ops, Is.EqualTo(Operation.Execute));
            });
        }

        [Test]
        public void Needs_HasOnlySomeRights_ShouldReturnMissingRightsOnly()
        {
            //Arrange
            var user = new User(Guid.NewGuid(), "Username", [new Right("Resource", Operation.Read | Operation.Write)]);
            List<Right> requiredRights = [new Right("Resource", Operation.Execute | Operation.Write)];
            //Act
            var missingRights = user.Needs(requiredRights);
            //Assert
            Assert.That(missingRights.Count(), Is.EqualTo(1));
            var missingRight = missingRights.Single();
            Assert.Multiple(() =>
            {
                Assert.That(missingRight.Resource, Is.EqualTo("Resource"));
                Assert.That(missingRight.Ops, Is.EqualTo(Operation.Execute));
            });
        }

        [Test]
        public void Needs_HasAllRequiredRights_ShouldReturnEmptyList()
        {
            //Arrange
            var user = new User(Guid.NewGuid(), "Username", [new Right("Resource", Operation.Read | Operation.Write | Operation.Execute)]);
            List<Right> requiredRights = [new Right("Resource", Operation.Execute)];
            //Act
            var missingRights = user.Needs(requiredRights);
            //Assert
            Assert.That(missingRights.Count(), Is.EqualTo(0));
        }
    }
}
