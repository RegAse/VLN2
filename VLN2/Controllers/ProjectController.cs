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
using Ionic.Zip;
using System.Text;
using System.IO;

namespace VLN2.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectsService _service = new ProjectsService();
        private UserService _userService = new UserService();

        // GET: Project
        [Authorize]
        public ActionResult Index(int id)
        {
            Project project = _service.GetProjectByID(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            string name = User.Identity.GetDisplayname();

            var model = new ProjectViewModel(project, name);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult DownloadZip(int id)
        {
            ZipFile zipFile = new ZipFile();

            Project project = _service.GetProjectByID(id);
            IEnumerable<ProjectFile> projectFiles = _service.GetProjectFilesByProjectID(id);

            foreach (ProjectFile projectFile in projectFiles)
            {
                zipFile.AddEntry(projectFile.Name, projectFile.Content);
            }

            string pathSafeProjectName = project.Name.ToLower().Trim().Replace(' ', '-');

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("content-disposition", "attachment; filename=" + pathSafeProjectName + ".zip");

            zipFile.Save(Response.OutputStream);
            zipFile.Dispose();
            
            return null;
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddCollaborator(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            int UserID = User.Identity.GetUserId<int>();
            var role = db.UserHasProject.Single(x => x.UserID == UserID && x.ProjectID == id);

            int roleID = role.ProjectRoleID;
            if (roleID != 1)
            {
                Response.Redirect("/Project/" + id);
                return null;
            }

            var user = db.Users.Single(x => x.Id == UserID);
            var following = user.Following.Intersect(user.Followers);
            //var following = _userService.GetFollowingByUsername(Username);
            var project = _service.GetProjectByID((int)id);

            var filtered = db.UserHasProject
                .Where(x => x.ProjectID == project.ID)
                .Select(y => y.ApplicationUser);
            var usersNotInProjectButAreFriends = following.Except(filtered);

            var model = new CollabaratorViewModel(UserID, usersNotInProjectButAreFriends, project, filtered);

            return View(model);
        }

        [HttpPost]
        [ActionName("AddCollaborator")]
        [Authorize]
        public ActionResult AddCollaboratorPost(int id, FormCollection Form)
        {
            //get userid from form
            int FriendUserID = Convert.ToInt32(Form["user"]);

            ApplicationDbContext db = new ApplicationDbContext();
            //get user and project from db
            var friend = db.Users.Single(x => x.Id == FriendUserID);
            Project Project = db.Projects.Single(x => x.ID == id);

            //Creates the relation from the user to the project
            UserHasProject userHasProject = new UserHasProject { ApplicationUser = friend, Project = Project, ProjectRoleID = 2 };

            db.UserHasProject.Add(userHasProject);

            //Saves data to db
            db.SaveChanges();
            
            //Proceeds to load the page
            int UserID = User.Identity.GetUserId<int>();

            var user = db.Users.Single(x => x.Id == UserID);
            var following = user.Following.Intersect(user.Followers);

            var filtered = db.UserHasProject
                .Where(x => x.ProjectID == Project.ID)
                .Select(y => y.ApplicationUser);
            var usersNotInProjectButAreFriends = following.Except(filtered);

            var model = new CollabaratorViewModel(UserID, usersNotInProjectButAreFriends, Project, filtered);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveCollaborator(int id, FormCollection Form)
        {
            int FriendUserID = Convert.ToInt32(Form["user"]);

            ApplicationDbContext db = new ApplicationDbContext();

            var friend = db.Users.Single(x => x.Id == FriendUserID);
            Project Project = db.Projects.Single(x => x.ID == id);

            var userHasProject = db.UserHasProject.Single(x => (x.ProjectID == Project.ID) && (x.UserID == friend.Id));

            //UserHasProject userHasProject = new UserHasProject { ApplicationUser = friend, Project = Project, ProjectRoleID = 2 };

            db.UserHasProject.Remove(userHasProject);

            db.SaveChanges();

            Response.Redirect("/Project/AddCollaborator/" + id.ToString());

            return null;
        }
    }
}