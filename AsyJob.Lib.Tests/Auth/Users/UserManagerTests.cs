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
        public async Task Ban_HasUserRights_ShouldAddUserToBanlist()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository();
            var fakeBans = new FakeBans();
            var user = new User(Guid.NewGuid(), "Senta", [
                new Right("Users", Operation.Write)
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, fakeBans, user);
            //Act
            await sut.Ban("troll@hotmail.com");
            //Assert
            Assert.That(fakeBans.BannedEmails, Contains.Item("troll@hotmail.com"));
        }

        [Test]
        public void Ban_MissingRight_ShouldThrowException()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository();
            var fakeBans = new FakeBans();
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, fakeBans);
            //Act
            async Task ban() => await sut.Ban("troll@hotmail.com");
            //Assert
            Assert.ThrowsAsync<UnauthorizedException>(ban, "User with missing right was able to ban user");
            Assert.That(fakeBans.BannedEmails, Is.Empty);
        }

        [Test]
        public void Whitelist_MissingRight_ShouldThrowException()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository();
            var fakeWhitelist = new FakeWhitelist();
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, fakeWhitelist, null!);
            //Act
            async Task whitelist() => await sut.Whitelist("literallyJFC@heaven.com");
            //Assert
            Assert.ThrowsAsync<UnauthorizedException>(whitelist, "User with missing right was able to whitelist user");
            Assert.That(fakeWhitelist.AllowedEmails, Is.Empty); 
        }

        [Test]
        public async Task Whitelist_IsAuthorized_ShouldWhitelistUser()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository();
            var fakeWhitelist = new FakeWhitelist();
            var user = new User(Guid.NewGuid(), "User", [
                new Right("Users", Operation.Write)
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, fakeWhitelist, null!, user);
            //Act
            await sut.Whitelist("literallyJFC@heaven.com");
            //Assert
            Assert.That(fakeWhitelist.AllowedEmails, Contains.Item("literallyJFC@heaven.com")); 
        }

        [Test]
        public void GetAll_MissingRights_ShouldThrowException()
        {
            //Arrange
            var fakeRepo = new FakeUserRepository([
                new User(Guid.NewGuid(), "User1", []),
                new User(Guid.NewGuid(), "Steve", [new Right("Billing", Operation.Read | Operation.Write | Operation.Execute)])
            ]);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!);
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
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!, user);
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
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!);
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
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!, user);
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
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!, user);
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
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, null!, user);
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
