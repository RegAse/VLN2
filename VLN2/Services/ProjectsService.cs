using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.Services
{
    public class ProjectsService
    {

        private ApplicationDbContext _db;

        public ProjectsService()
        {
            _db = new ApplicationDbContext();
        }

        public Project GetProjectByID(int projectID)
        {
            var project = _db.Projects.SingleOrDefault(x => x.ID == projectID);

            if (project == null)
            {
                // Throw Not Found
            }

            return project;
        }

        public IEnumerable<Project> GetProjectsByUserID(int userID)
        {
            IEnumerable<Project> Projects = _db.Users.Single(x => x.Id == userID).Projects;

            return Projects;
        }
    }
}