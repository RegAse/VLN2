using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLN2.Models;

namespace VLN2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (IfLoggedIn)
            {
                Response.Redirect("/Dashboard");
                return null;
            }
            return View();
        }
    }
}