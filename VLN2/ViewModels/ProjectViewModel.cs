using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace VLN2.ViewModels
{
    public class ProjectViewModel
    {
        public string Displayname { get; set; }

        public Project Project { get; set; }

        public ProjectViewModel(Project project, string displayname)
        {
            Project = project;

            Displayname = displayname;
        }

    }
}