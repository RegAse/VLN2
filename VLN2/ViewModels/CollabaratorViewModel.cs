using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class CollabaratorViewModel
    {
        public int UserID;
        public IEnumerable<ApplicationUser> Following;
        public Project Project;
        public IEnumerable<ApplicationUser> UsersInProject;

        public CollabaratorViewModel(int userID, IEnumerable<ApplicationUser> following, Project project, IEnumerable<ApplicationUser> usersInProject)
        {
            UserID = userID;
            Following = following;
            Project = project;
            UsersInProject = usersInProject;
        }
    }
}