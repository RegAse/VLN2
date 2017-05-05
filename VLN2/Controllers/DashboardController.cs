using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLN2.Extensions;
using VLN2.Models;
using VLN2.Services;
using VLN2.ViewModels;

namespace VLN2.Controllers
{
    public class DashboardController : Controller
    {
        private ProjectsService _service = new ProjectsService();

        // GET: Dashboard
        public ActionResult Index()
        {
            int userID = User.Identity.GetUserId<int>();
            var projects = _service.GetProjectsByUserID(userID);
            var model = new DashboardViewModel(userID, projects);
            return View(model);
        }
    }
}