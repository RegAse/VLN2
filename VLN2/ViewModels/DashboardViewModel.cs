using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class DashboardViewModel
    {
        public string Username;

        IEnumerable<Project> UserProjects;

        DashboardViewModel(string username, IEnumerable<Project> userProjects)
        {
            Username = username;

            UserProjects = userProjects;
        }
    }
}