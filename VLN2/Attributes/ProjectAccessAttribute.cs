using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VLN2.Models;
using VLN2.Services;

namespace VLN2.Attributes
{
    public class ProjectAccessAttribute : AuthorizeAttribute
    {
        private ProjectService _service = new ProjectService();
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var id = int.Parse(httpContext.Request.RequestContext.RouteData.Values["id"].ToString());
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            ApplicationDbContext db = new ApplicationDbContext();
            int UserID = httpContext.User.Identity.GetUserId<int>();

            // Get all roles the user has in the project
            var roles = db.UserHasProject.Where(x => x.UserID == UserID && x.ProjectID == id).Select(y => y.ProjectRole.Name);

            // if the user has creator role he can access all routes that a normal user can access
            if (roles.Contains("Creator"))
            {
                return true;
            }

            return roles.Contains(this.AccessLevel);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Error",
                                action = "Unauthorised"
                            })
                        );
        }
    }
}