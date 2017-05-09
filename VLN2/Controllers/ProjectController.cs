using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLN2.Models;
using VLN2.Services;
using VLN2.ViewModels;
using VLN2.Extensions;

namespace VLN2.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectsService _service = new ProjectsService();

        // GET: Project
        public ActionResult Index(int ?id)
        {
            Project project = _service.GetProjectByID((int)id);
            if (id == null || project == null)
            {
                return HttpNotFound();
            }

            string name = User.Identity.GetDisplayname();

            var model = new ProjectViewModel(project, name);

            return View(model);
        }
    }
}