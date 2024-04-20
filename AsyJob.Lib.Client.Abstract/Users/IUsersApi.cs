using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Client.Abstract.Users
{
    public interface IUsersApi
    {
        /// <summary>
        /// Return a list of all users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserResponse>> GetAll();

        Task Whitelist(WhitelistRequest request);
        Task Ban(BanRequest request);
        Task<UserResponse> Update(UserUpdateRequest request);
    }
}
