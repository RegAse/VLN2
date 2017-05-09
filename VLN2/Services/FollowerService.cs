using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.Services
{
    public class FollowerService
    {
        private ApplicationDbContext _db;

        public FollowerService()
        {
            _db = new ApplicationDbContext();
        }

        /*public Follower GetUserByFollower(string userNameID)
        {
            var user = _db.Users.SingleOrDefault(x => x.Description == userNameID);

            return user;
        }
        public Follower GetFollowerByUser(string userName)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName);

            return user;
        }*/

    }
}