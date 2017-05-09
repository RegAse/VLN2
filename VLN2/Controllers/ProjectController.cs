﻿using Microsoft.AspNet.Identity;
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
            if (id == null)
            {
                return HttpNotFound();
            }

            //ApplicationDbContext appDb = new ApplicationDbContext();
            //int user = User.Identity.GetUserId<int>();
            //string name = appDb.Users.Single(x => x.Id == user).Displayname;

            string name = User.Identity.GetDisplayname();

            //int user = User.Identity.GetUserId<int>();
            //var model = new ProjectViewModel(_service.GetProjectsByUserID(user).Single(), name);
            var model = new ProjectViewModel(_service.GetProjectByID((int)id), name);

            return View(model);
        }
    }
}