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
        private UserService _userService = new UserService();

        // GET: Project
        public ActionResult Index(int ?id)
        {
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (!IfLoggedIn)
            {
                Response.Redirect("/Account/Login");
                return null;
            }

            Project project = _service.GetProjectByID((int)id);
            if (id == null || project == null)
            {
                return HttpNotFound();
            }

            string name = User.Identity.GetDisplayname();

            var model = new ProjectViewModel(project, name);

            return View(model);
        }

        [HttpGet]
        public ActionResult AddCollaborator(int ?id)
        {
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (!IfLoggedIn)
            {
                Response.Redirect("/Account/Login");
                return null;
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            ApplicationDbContext db = new ApplicationDbContext();

            string Username = User.Identity.GetUserName();
            int UserID = User.Identity.GetUserId<int>();

            var user = db.Users.Single(x => x.Id == UserID);
            var following = user.Following.Intersect(user.Followers);
            //var following = _userService.GetFollowingByUsername(Username);
            var project = _service.GetProjectByID((int)id);

            var filtered = db.UserHasProject
                .Where(x => x.ProjectID == project.ID)
                .Select(y => y.ApplicationUser);
            var usersNotInProjectButAreFriends = following.Except(filtered);

            var model = new CollabaratorViewModel(UserID, usersNotInProjectButAreFriends, project);

            return View(model);
        }

        [HttpPost]
        [ActionName("AddCollaborator")]
        public ActionResult AddCollaboratorPost(int ?id, FormCollection Form)
        {
            bool IfLoggedIn = User.Identity.IsAuthenticated;
            if (!IfLoggedIn)
            {
                Response.Redirect("/Account/Login");
                return null;
            }

            if (id == null)
            {
                return HttpNotFound();
            }
            
            //get userid from form
            int FriendUserID = Convert.ToInt32(Form["user"]);

            ApplicationDbContext db = new ApplicationDbContext();
            //get user and project from db
            var friend = db.Users.Single(x => x.Id == FriendUserID);
            Project Project = db.Projects.Single(x => x.ID == id);

            //Creates the relation from the user to the project
            UserHasProject userHasProject = new UserHasProject { ApplicationUser = friend, Project = Project, ProjectRoleID = 1 };

            db.UserHasProject.Add(userHasProject);

            //Saves data to db
            db.SaveChanges();
            
            //Proceeds to load the page
            string Username = User.Identity.GetUserName();
            int UserID = User.Identity.GetUserId<int>();

            var user = db.Users.Single(x => x.Id == UserID);
            var following = user.Following.Intersect(user.Followers);
            //var following = _userService.GetFollowingByUsername(Username);
            var project = _service.GetProjectByID((int)id);

            var filtered = db.UserHasProject
                .Where(x => x.ProjectID == Project.ID)
                .Select(y => y.ApplicationUser);
            var usersNotInProjectButAreFriends = following.Except(filtered);

            var model = new CollabaratorViewModel(UserID, usersNotInProjectButAreFriends, Project);

            return View(model);
        }
    }
}