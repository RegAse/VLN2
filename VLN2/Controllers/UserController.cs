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
            var user = _service.GetIdByUsername(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            //Get data for the viewModel and put into the var model
            string name = _service.GetIdByUsername(username).Displayname;

            string description = _service.GetIdByUsername(username).Description;

            string userName = _service.GetIdByUsername(username).UserName;

            ICollection<ApplicationUser> following = _service.GetFollowingByUsername(username);

            ICollection<ApplicationUser> followers = _service.GetFollowersByUsername(username);

            var model = new UserViewModel(name, description, userName, followers, following);
            return View(model);
        }

        public ActionResult Index(string username, FormCollection collection)
        {
            return null;
        }

    }
}