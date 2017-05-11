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

        public CollabaratorViewModel(int userID, IEnumerable<ApplicationUser> following, Project project)
        {
            UserID = userID;
            Following = following;
            Project = project;
        }
    }
}