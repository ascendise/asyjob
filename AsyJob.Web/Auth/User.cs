﻿using AspNetCore.Identity.MongoDbCore.Models;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Runner;

namespace AsyJob.Web.Auth
{
    public class User : MongoIdentityUser<Guid>
    {
        public IEnumerable<Right> Rights { get; set; } = [
            new Right(nameof(JobRunner), Operation.Read | Operation.Write | Operation.Execute)
        ];

        public User(string username) : base(username) { }
        public User() : base() { }

        public Lib.Auth.User GetDomainUser()
        {
            return new(Id, UserName!, Rights);
        }
    }
}