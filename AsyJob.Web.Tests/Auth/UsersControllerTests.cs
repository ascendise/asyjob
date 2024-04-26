using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Users;
using AsyJob.Lib.Tests.TestDoubles;
using AsyJob.Web.Auth;
using AsyJob.Web.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Web.Tests.Auth
{
    internal class UsersControllerTests
    {
        private Web.Auth.User _admin = new("admin")
        {
            Rights = [
                new Right("Users", Operation.Read | Operation.Write)
            ]
        };

        [Test]
        public async Task GetUsers_HasUsers_ShouldReturnListOfUsers()
        {
            //Arrange
            var user = new Web.Auth.User("HelloWorld");
            var fakeUserRepo = new FakeUserRepository([_admin.GetDomainUser(), user.GetDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.GetDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, user]);
            var sut = new UsersController(usersApi, fakeAspUserManager, new UserResponseToHalMapper(_admin));
            //Act
            var users = await sut.GetUsers();
            //Assert
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUsers_Response_ShouldIncludeLinks()
        {
            //Arrange
            var fakeUserRepo = new FakeUserRepository([_admin.GetDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.GetDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin]);
            var sut = new UsersController(usersApi, fakeAspUserManager, new UserResponseToHalMapper(_admin));
            //Act
            var response = await sut.GetUsers();
            //Assert
            var userResponse = response.Single();
            Assert.That(userResponse.Links.Any(l => l.Key == "users"), "users link missing");
            Assert.That(userResponse.Links.Any(l => l.Key == "user"), "user link missing");
        }

        [Test]
        public async Task GetUsers_UnconfirmedUser_ShouldIncludeConfirmLink()
        {
            //Arrange
            var unconfirmedUser = new Web.Auth.User("Unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([_admin.GetDomainUser(), unconfirmedUser.GetDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.GetDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, unconfirmedUser]);
            var sut = new UsersController(usersApi, fakeAspUserManager, new UserResponseToHalMapper(_admin));
            //Act
            var response = await sut.GetUsers();
            //Assert
            var userResponse = response.Single(u => u.Username == "Unconfirmed");
            Assert.That(userResponse.Links.Any(l => l.Key == "confirm"), "confirm link missing");
        }

        [Test]
        //Confirming is a write action, so the readonly user does not care about it

        public async Task GetUsers_ReadOnlyUserRights_ShouldNotIncludeConfirmLink()
        {
            //Arrange
            var readonlyUser = new Web.Auth.User("ReadOnly")
            {
                ConfirmedByAdmin = true,
                Rights = [new Right("Users", Operation.Read)]
            };
            var unconfirmedUser = new Web.Auth.User("Unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([readonlyUser.GetDomainUser(), unconfirmedUser.GetDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, readonlyUser.GetDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([readonlyUser, unconfirmedUser]);
            var sut = new UsersController(usersApi, fakeAspUserManager, new UserResponseToHalMapper(readonlyUser));
            //Act
            var response = await sut.GetUsers();
            //Assert
            var userResponse = response.Single(u => u.Username == "Unconfirmed");
            Assert.That(!userResponse.Links.Any(l => l.Key == "confirm"), 
                "confirm link is present, but user does not have required rights");
        }

    }
}
