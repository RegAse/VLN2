using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VLN2.Services;
using VLN2.Models;
using VLN2.ViewModels;
using VLN2.Extensions;

namespace VLN2.Controllers
{
    public class UserController : Controller
    {
        private UserService _service = new UserService();
        // GET: User
        public ActionResult Index(string username)
        {
            //Check if the user is empty
            var user = _service.GetUserByUsername(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            //Get data for the viewModel and put into the var model
            int userid = _service.GetUserByUsername(username).Id;

            string name = _service.GetUserByUsername(username).Displayname;

            string description = _service.GetUserByUsername(username).Description;

            ICollection<ApplicationUser> following = _service.GetFollowingByUsername(username);

            ICollection<ApplicationUser> followers = _service.GetFollowersByUsername(username);

            var model = new UserViewModel(userid, name, description, username, followers, following);
            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult AddFollower(string username, FormCollection collection)
        {
            int followerID = Convert.ToInt32(collection["userId"]);
            _service.AddFollower(User.Identity.GetUserId<int>(), followerID);
            return Index(username);
        }

        public ActionResult RemoveFollower(FormCollection collection)
        {
            int followerID = Convert.ToInt32(collection["userId"]);
            _service.RemoveFollower(User.Identity.GetUserId<int>(), followerID);

            ApplicationUser username = _service.GetUserByUserID(followerID);

            Response.Redirect("/user/profile/" + username.UserName);
            return null;
        }
    }
}