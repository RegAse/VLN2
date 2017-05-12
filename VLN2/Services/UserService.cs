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
            return _db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        public ApplicationUser getUserByUserId(int userid)
        {
            return _db.Users.SingleOrDefault(x => x.Id == userid);
        }
        public ICollection<ApplicationUser> GetFollowingByUsername(string userName)
        {
            return _db.Users.Single(x => x.UserName == userName).Following;
        }
        public ICollection<ApplicationUser> GetFollowersByUsername(string userName)
        {
            return _db.Users.Single(x => x.UserName == userName).Followers;
        }

        public void AddFollower(int userId, int followerId)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userId);
            var userToFollow = users.Single(y => y.Id == followerId);
            
            user.Following.Add(userToFollow);
            
            _db.SaveChanges();
        }

        public void RemoveFollower(int userId, int followerId)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userId);
            var userToFollow = users.Single(y => y.Id == followerId);

            user.Following.Remove(userToFollow);

            _db.SaveChanges();
        }

        public ApplicationUser GetUserByUserID(int userID)
        {
            return _db.Users.SingleOrDefault(x => x.Id == userID);
        }

        public ApplicationUser UpdateBio(int userid, string desc)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userid);

            user.Description = desc;
            _db.SaveChanges();

            return user;
        }
    }
}