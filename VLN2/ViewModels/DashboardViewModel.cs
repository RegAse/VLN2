using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class DashboardViewModel
    {
        int UserID;
        IEnumerable<Project> UserProjects;

        public DashboardViewModel(int userID, IEnumerable<Project> projects)
        {
            UserID = userID;
            UserProjects = projects;
        }

        /*DashboardViewModel(string username, IEnumerable<Project> userProjects)
        {
            Username = username;

            UserProjects = userProjects;
        }*/
    }
}