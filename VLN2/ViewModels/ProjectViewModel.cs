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

        public IEnumerable<Project> Projects { get; set; }

        public ProjectViewModel(IEnumerable<Project> projects, Project project)
        {
            Projects = projects;
            Project = project;
        }

    }
}