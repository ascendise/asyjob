using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Abstract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Client.Users
{
    public class UsersApi(IUserManager userManager) : IUsersApi
    {
        private readonly IUserManager _userManager = userManager;
        private readonly UserMapper _userMapper = new();

        public async Task Ban(BanRequest request)
            => await _userManager.Ban(request.Email);

        public async Task Whitelist(WhitelistRequest request)
            => await _userManager.Whitelist(request.Email);

        public async Task<IEnumerable<UserResponse>> GetAll()
            => (await _userManager.GetAll()).Select(_userMapper.Map);

        public async Task<UserResponse> Update(UserUpdateRequest request)
        {
            var newUser = await _userManager.Update(request.Id, _userMapper.Map(request));
            return _userMapper.Map(newUser);
        }
    }
}
