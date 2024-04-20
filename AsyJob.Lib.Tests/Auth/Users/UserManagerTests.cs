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
            var user = new User(Guid.NewGuid(), "User", []);
            var sut = new UserManager(new AuthorizationManager(), fakeRepo, null!, fakeBans, user);
            //Act
            var ban = async () => await sut.Ban("troll@hotmail.com");
            //Assert
            Assert.ThrowsAsync<UnauthorizedException>(() => ban(), "User with missing right was able to ban user");
        }

    }
}
