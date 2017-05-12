using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLN2.Models;

namespace VLN2.Tests.ServiceClasses
{
    public class UserService
    {
        private IAppDataContext _db;

        public UserService(IAppDataContext context)
        {
            _db = context;
        }
/*
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>User associated with the username</returns>
        public ApplicationUser GetUserByUsername(string username)
        {
            return _db.Users.SingleOrDefault(x => x == username);
        }

        /// <summary>
        /// Get the user by userID
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>User associated with the userID</returns>
        public ApplicationUser GetUserByUserID(int userID)
        {
            return _db.Users.SingleOrDefault(x => x.Id == userID);
        }

        /// <summary>
        /// Get all following of user by username
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <returns>All following of the user</returns>
        public ICollection<ApplicationUser> GetFollowingByUsername(string userName)
        {
            return _db.Users.SingleOrDefault(x => x.UserName == userName).Following;
        }

        /// <summary>
        /// Get all followers of user by username
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <returns>All followers of the user</returns>
        public ICollection<ApplicationUser> GetFollowersByUsername(string userName)
        {
            return _db.Users.SingleOrDefault(x => x.UserName == userName).Followers;
        }

        /// <summary>
        /// Add follower
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="followerId">The ID of the user to follow</param>
        public void AddFollower(int userId, int followerId)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userId);
            var userToFollow = users.Single(y => y.Id == followerId);

            user.Following.Add(userToFollow);

            _db.SaveChanges();
        }

        /// <summary>
        /// Remove a follower
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="followerId">The ID of the user to unfollow</param>
        public void RemoveFollower(int userId, int followerId)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userId);
            var userToFollow = users.Single(y => y.Id == followerId);

            user.Following.Remove(userToFollow);

            _db.SaveChanges();
        }

        /// <summary>
        /// Updates bio
        /// </summary>
        /// <param name="userid">The ID of the user</param>
        /// <param name="desc">The new description of the user</param>
        /// <returns>Returns the new user</returns>
        public ApplicationUser UpdateBio(int userid, string desc)
        {
            var users = _db.Users;
            var user = users.Single(x => x.Id == userid);

            user.Description = desc;
            _db.SaveChanges();

            return user;
        }*/
    }
}
