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

        public DashboardViewModel(int userID, IEnumerable<Project> projects)
        {
            UserID = userID;
            UserProjects = projects;
        }
    }
}