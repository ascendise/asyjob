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
        private Web.Auth.User _admin = new Web.Auth.User("admin")
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
            var sut = new UsersController(usersApi, fakeAspUserManager);
            //Act
            var users = await sut.GetUsers();
            //Assert
            Assert.That(users.Count(), Is.EqualTo(2));
        }

    }
}
