using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }

        public ProjectViewModel(Project project)
        {
            Project = project;
        }

    }
}