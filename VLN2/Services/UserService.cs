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

        public ApplicationUser GetDataByUsername(string userName)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return null;
            }

            return user;
        }
        public ICollection<ApplicationUser> GetFollowingByUsername(string userName)
        {
            ICollection<ApplicationUser> users = _db.Users.Single(x => x.UserName == userName).Following;
            return users;
        }
        public ICollection<ApplicationUser> GetFollowersByUsername(string userName)
        {
            ICollection<ApplicationUser> users = _db.Users.Single(x => x.UserName == userName).Followers;
            return users;
        }

        //public ICollection<ApplicationUser> GetFriendsByUsername(string userName)
        //{
        //    ICollection<ApplicationUser> friends = _db.Users.Single(x => x.UserName == )
        //}

        public void AddFollower(int userId, int followerId)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userId);
            var userToFollow = users.Single(y => y.Id == followerId);
            
            
            user.Following.Add(userToFollow);
            
            _db.SaveChanges();
        }
    }
}