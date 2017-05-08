using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.Services
{
    public class UserService
    {
        private ApplicationDbContext _db;

        public UserService()
        {
            _db = new ApplicationDbContext();
        }

        public ApplicationUser GetIdByUsername(string userName)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                // Throw Not Found
            }

            return user;
        }
    }
}