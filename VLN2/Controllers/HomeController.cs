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
            return View();
        }

        public ActionResult About()
        {
            /*int userID = User.Identity.GetUserId<int>();

            ApplicationDbContext db = new ApplicationDbContext();
            var projects = db.Projects;

            var project = projects.Single(x => x.ID == 1);
            var user = db.Users.Single(x => x.Id == userID);

            UserHasProject userHasProject = new UserHasProject { ApplicationUser = user, Project = project, ProjectRoleID = 1 };
            db.UserHasProject.Add(userHasProject);
            */

            /* Test adding followers */
            ApplicationDbContext db = new ApplicationDbContext();
            var users = db.Users;
            var user = users.Single(x => x.Id == 1);
            var userToFollow = users.Single(y => y.Id == 2);

            /*Follower follower = new Follower();
            follower.UserID = 1;
            follower.UserFollowID = 2;
            */
            user.Followers.Add(userToFollow);

            db.SaveChanges();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat(int ?id)
        {
            return View();
        }
    }
}