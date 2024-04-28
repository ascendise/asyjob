using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Lib.Client.Users;
using AsyJob.Lib.Tests.TestDoubles;
using AsyJob.Web.Auth;
using AsyJob.Web.Tests.TestDoubles;
using MongoDB.Driver;
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
            ConfirmedByAdmin = true,
            Rights = [
                new Right("Users", Operation.Read | Operation.Write)
            ]
        };

        [Test]
        public async Task GetUsers_HasUsers_ShouldReturnListOfUsers()
        {
            //Arrange
            var user = new Web.Auth.User("HelloWorld");
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), user.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, user]);
            var sut = new UsersController(usersApi, fakeAspUserManager, 
                new UserToUserResponseMapper(), new UserResponseToHalMapper(_admin));
            //Act
            var users = await sut.GetUsers();
            //Assert
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUsers_Response_ShouldIncludeLinks()
        {
            //Arrange
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var response = await sut.GetUsers();
            //Assert
            var userResponse = response.Single();
            Assert.That(userResponse.Links.Any(l => l.Key == "users"), "users link missing");
            Assert.That(userResponse.Links.Any(l => l.Key == "self"), "user link missing");
        }

        [Test]
        public async Task GetUsers_UnconfirmedUser_ShouldIncludeConfirmLink()
        {
            //Arrange
            var unconfirmedUser = new Web.Auth.User("Unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), unconfirmedUser.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, unconfirmedUser]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
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
            var fakeUserRepo = new FakeUserRepository([readonlyUser.ToDomainUser(), unconfirmedUser.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, readonlyUser.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([readonlyUser, unconfirmedUser]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(readonlyUser));
            //Act
            var response = await sut.GetUsers();
            //Assert
            var userResponse = response.Single(u => u.Username == "Unconfirmed");
            Assert.That(!userResponse.Links.Any(l => l.Key == "confirm"), 
                "confirm link is present, but user does not have required rights");
        }

        [Test]
        public async Task Update_UserExists_ShouldUpdateUser()
        {
            //Arrange
            var user = new Web.Auth.User("User");
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), user.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, user]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var updateRequest = new UserUpdateRequest(user.Id, "NewUsername", ["Test_rwx"]);
            var response = await sut.Update(user.Id, updateRequest);
            //Assert
            var newUser = fakeUserRepo.Users.Single(u => u.Id == user.Id);
            Assert.That(newUser.Username, Is.EqualTo("NewUsername"));
            Assert.Multiple(() =>
            {
                Assert.That(newUser.Rights.Single().Resource, Is.EqualTo("Test"));
                Assert.That(newUser.Rights.Single().Ops, Is.EqualTo(Operation.Read | Operation.Write | Operation.Execute));
            });
        }

        [Test]
        public void Update_UserDoesNotExist_ShouldThrowException()
        {
            //Arrange
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var userId = Guid.Parse("686131d6-2bf2-4d7e-8700-405fdd7ccd57");
            var updateRequest = new UserUpdateRequest(userId, "NewUsername", ["Test_rwx"]);
            var update = async () => await sut.Update(userId, updateRequest);
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => update());
        }

        [Test]
        public async Task Update_UserExists_ShouldIncludeLinksInResponse()
        {
            //Arrange
            var user = new Web.Auth.User("User")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), user.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, user]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var updateRequest = new UserUpdateRequest(user.Id, "NewUsername", ["Test_rwx"]);
            var response = await sut.Update(user.Id, updateRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Links.Any(l => l.Key == "self"), "self link missing");
                Assert.That(response.Links.Any(l => l.Key == "users"), "users link missing");
                Assert.That(response.Links.Any(l => l.Key == "confirm"), "confirm link missing");
            });
        }

        [Test]
        public async Task GetUnconfirmedUsers_HasUsers_ShouldReturnUsers()
        {
            //Arrange
            var unconfirmed = new Web.Auth.User("unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), unconfirmed.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, unconfirmed]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var response = await sut.GetUnconfirmedUsers();
            //Assert
            Assert.That(response.Single().Username, Is.EqualTo(unconfirmed.UserName));
        }

        [Test]
        public async Task GetUnconfirmedUsers_Links_ShouldContainCorrectLinks()
        {
            //Arrange
            var unconfirmed = new Web.Auth.User("unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var fakeUserRepo = new FakeUserRepository([_admin.ToDomainUser(), unconfirmed.ToDomainUser()]);
            var userManager = new UserManager(new AuthorizationManager(), fakeUserRepo, _admin.ToDomainUser());
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, unconfirmed]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var response = await sut.GetUnconfirmedUsers();
            //Assert
            var userResponse = response.Single();
            Assert.That(userResponse.Links.Any(l => l.Key == "self"), "self is missing");
            Assert.That(userResponse.Links.Any(l => l.Key == "users"), "users is missing");
            Assert.That(userResponse.Links.Any(l => l.Key == "confirm"), "confirm is missing");
        }

        [Test]
        public async Task ConfirmUser_UserExists_ShouldActivateUser()
        {
            //Arrange
            var unconfirmed = new Web.Auth.User("unconfirmed")
            {
                ConfirmedByAdmin = false
            };
            var userManager = new UserManager(null!, null!, null!);
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin, unconfirmed]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            await sut.ConfirmUser(unconfirmed.Id, new(["Test_x"]));
            //Assert
            Assert.That(unconfirmed, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(unconfirmed.ConfirmedByAdmin);
                Assert.That(unconfirmed.Rights.Single(), Is.EqualTo(new Right("Test", Operation.Execute)));
            });
        }

        [Test]
        public void ConfirmUser_UserDoesNotExist_ShouldActivateUser()
        {
            //Arrange
            var userManager = new UserManager(null!, null!, null!);
            var usersApi = new UsersApi(userManager);
            var fakeAspUserManager = new FakeAspUserManager([_admin]);
            var sut = new UsersController(usersApi, fakeAspUserManager,  
                new UserToUserResponseMapper(),new UserResponseToHalMapper(_admin));
            //Act
            var update = async ()
                => await sut.ConfirmUser(Guid.Parse("191f1ad0-5360-4f86-8fcd-8099981a4ce2"), new(["Test_x"]));
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => update());
        }
    }
}
