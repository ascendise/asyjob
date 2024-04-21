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

        /// <summary>
        /// Allows email access to the api
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Whitelist(WhitelistRequest request);
        /// <summary>
        /// Bans email from accessing api
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Ban(BanRequest request);
        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserResponse> Update(UserUpdateRequest request);
        Task<UserAccessResponse> GetUserAccessRights(GetUserAccessRightsRequest request);
    }
}
