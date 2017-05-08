using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VLN2.Services;
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
            if (username == null)
            {
                // Throw 404 exception
            }
            string name = User.Identity.GetUserName();
            string description = User.Identity.GetDescription();

            var model = new UserViewModel(name, description);
            return View(model);
        }

        // POST: User

    }
}