using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class DashboardViewModel
    {
        public int UserID;
        public IEnumerable<Project> UserProjects;
        public IEnumerable<Project> InvolvedProjects;

        public DashboardViewModel(int userID, IEnumerable<Project> projects, IEnumerable<Project> involvedProjects)
        {
            UserID = userID;
            UserProjects = projects;
            InvolvedProjects = involvedProjects;
        }
    }
}