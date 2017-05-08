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

            var model = new UserViewModel(_service.GetIdByUsername(), name);
            return View();
        }

        // POST: User

    }
}