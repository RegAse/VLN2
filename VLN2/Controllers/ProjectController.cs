using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLN2.Models;
using VLN2.Services;
using VLN2.ViewModels;

namespace VLN2.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectsService _service = new ProjectsService();

        // GET: Project
        public ActionResult Index(int ?id)
        {
            if (id == null)
            {
                // Throw error
            }
            var model = new ProjectViewModel(_service.GetProjectByID((int)id));

            return View(model);
        }
    }
}