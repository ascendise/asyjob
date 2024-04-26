using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Abstract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Client.Users
{
    internal class UserMapper :
        IMapper<User, UserResponse>,
        IMapper<UserUpdateRequest, UserUpdate>
    {
        public UserResponse Map(User user)
            => new(
                user.Id, user.Username, 
                user.Rights.Select(r => r.ToString()),
                user.Active);

        public UserUpdate Map(UserUpdateRequest req)
            => new(req.Username, req.Rights?.Select(r => new Right(r)));
    }
}
