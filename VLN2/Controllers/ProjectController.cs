using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLN2.Models;
using VLN2.ViewModels;

namespace VLN2.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var model = new ProjectViewModel(db.Projects);

            return View(model);
        }
    }
}