using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.Auth.Users
{
    internal class UserManagerTests
    {
        [Test]
        public void GetAll_MissingRights_ShouldThrowException()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository([
                new User(Guid.NewGuid(), "User1", []),
                new User(Guid.NewGuid(), "Steve", [new Right("Billing", Operation.Read | Operation.Write | Operation.Execute)])
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo);
            //Act
            async Task<IEnumerable<User>> getAllUsers() => await sut.GetAll();
            //Assert
            Assert.ThrowsAsync<UnauthorizedException>(getAllUsers, "User with missing rights was able to fetch users");
        }


        [Test]
        public async Task GetAll_HasRights_ShouldReturnUsers()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository([
                new User(Guid.NewGuid(), "User1", []),
                new User(Guid.NewGuid(), "Steve", [new Right("Billing", Operation.Read | Operation.Write | Operation.Execute)])
            ]);
            var user = new User(Guid.NewGuid(), "Admin", [new Right("Users", Operation.Read)]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, user);
            //Act
            var users = await sut.GetAll();
            //Assert
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Update_MissingRights_ShouldThrowException()
        {
            //Arrange
            var joeId = Guid.NewGuid();
            var fakeRepo = new FakeUserRepository([
                new User(joeId, "AverageJoe", [])
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo);
            //Act
            async Task<User> update(Guid userId, UserUpdate update) => await sut.Update(userId, update);
            //Assert
            var updateRequest = new UserUpdate("JosephDestroyerOfWorlds", [new Right("Nukes", Operation.Execute)]);
            Assert.ThrowsAsync<UnauthorizedException>(() => update(joeId, updateRequest),
                "User without rights was able to update user");
            Assert.That(fakeRepo.Users.Single().Username, Is.EqualTo("AverageJoe"));
        }

        [Test]
        public void Update_InvalidId_ShouldThrowKeyNotFoundException()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository([
                new User(Guid.NewGuid(), "AverageJoe", [])
            ]);
            var user = new User(Guid.NewGuid(), "Admin", [
                new Right("Users", Operation.Read | Operation.Write)
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, user);
            //Act
            var updateRequest = new UserUpdate("JosephDestroyerOfWorlds", [new Right("Nukes", Operation.Execute)]);
            var wrongId = Guid.NewGuid();
            async Task<User> update() => await sut.Update(wrongId, updateRequest);
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(update);
        }

        [Test]
        public async Task Update_HasRights_ShouldUpdateUser()
        {
            //Arrange
            var joeId = Guid.NewGuid();
            var fakeRepo = new FakeUserRepository([
                new User(joeId, "AverageJoe", [])
            ]);
            var user = new User(Guid.NewGuid(), "Admin", [
                new Right("Users", Operation.Read | Operation.Write)
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, user);
            //Act
            var updateRequest = new UserUpdate("JosephDestroyerOfWorlds", [new Right("Nukes", Operation.Execute)]);
            var newUser = await sut.Update(joeId, updateRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(newUser.Username, Is.EqualTo("JosephDestroyerOfWorlds"));
                Assert.That(newUser.Rights, Contains.Item(new Right("Nukes", Operation.Execute)));
            });
            var repoUser = fakeRepo.Users.Single();
            Assert.Multiple(() =>
            {
                Assert.That(repoUser.Username, Is.EqualTo("JosephDestroyerOfWorlds"));
                Assert.That(repoUser.Rights, Contains.Item(new Right("Nukes", Operation.Execute)));
            });
        }

        [Test]
        public async Task Update_PartialUpdate_ShouldOnlyUpdateUsername()
        {
            //Arrange
            var joeId = Guid.NewGuid();
            var fakeRepo = new FakeUserRepository([
                new User(joeId, "AverageJoe", [])
            ]);
            var user = new User(Guid.NewGuid(), "Admin", [
                new Right("Users", Operation.Read | Operation.Write)
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, user);
            //Act
            var updateRequest = new UserUpdate("SlightlyAboveAvgJoe");
            var newUser = await sut.Update(joeId, updateRequest);
            //Assert
            var repoUser = fakeRepo.Users.Single();
            Assert.Multiple(() =>
            {
                Assert.That(repoUser.Username, Is.EqualTo("SlightlyAboveAvgJoe"));
                Assert.That(repoUser.Rights.Count, Is.EqualTo(0));
            });
        }
    }
}
