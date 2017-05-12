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
        private ProjectService _service = new ProjectService();

        // GET: Dashboard
        [HttpGet]
        public ActionResult Index()
        {
            //login check
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (!IfLoggedIn)
            {
                Response.Redirect("/Account/Login");
                return null;
            }
            //Gets the userID
            int userID = User.Identity.GetUserId<int>();
            
            ApplicationDbContext db = new ApplicationDbContext();

            //Gets the users project which he created
            var YourProjects = db.UserHasProject
                .Where(x => x.ProjectRoleID == 1)
                .Where(y => y.UserID == userID)
                .Select(z => z.Project);
            //Gets projects the user is involved in
            var ProjectsInvolvedIn = db.UserHasProject
                .Where(x => x.ProjectRoleID == 2)
                .Where(y => y.UserID == userID)
                .Select(z => z.Project);

            var model = new DashboardViewModel(userID, YourProjects, ProjectsInvolvedIn);
            return View(model);
        }

        /// Processes the user creating a new project
        [HttpPost]
        public ActionResult Index(FormCollection Form)
        {
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (!IfLoggedIn)
            {
                Response.Redirect("/Account/Login");
                return null;
            }

            int userID = User.Identity.GetUserId<int>();

            //Gets the information from the "Create Project" form
            string ProjectName = Form["projectName"].ToString();
            string Description = Form["description"].ToString();
            string Filename = Form["fileName"].ToString();

            ApplicationDbContext db = new ApplicationDbContext();

            //Creates a new project from the form information
            Project project = new Project {Name = ProjectName, Description = Description};

            //Adds the project to the database
            db.Projects.Add(project);

            //Gets the user from the database
            var user = db.Users.Single(x => x.Id == userID);

            //Creates the file for the project
            var projectFile = new ProjectFile();

            projectFile.Name = Filename;
            projectFile.Content = "";

            project.ProjectFiles.Add(projectFile);

            //Creates the relation from the user to the project
            UserHasProject userHasProject = new UserHasProject { ApplicationUser = user, Project = project, ProjectRoleID = 1 };

            db.UserHasProject.Add(userHasProject);
            //Saves the changes to the db
            db.SaveChanges();

            var YourProjects = db.UserHasProject
                .Where(x => x.ProjectRoleID == 1)
                .Where(y => y.UserID == userID)
                .Select(z => z.Project);

            var ProjectsInvolvedIn = db.UserHasProject
                .Where(x => x.ProjectRoleID == 2)
                .Where(y => y.UserID == userID)
                .Select(z => z.Project);

            var model = new DashboardViewModel(userID, YourProjects, ProjectsInvolvedIn);

            return View(model);
        }
    }
}