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

            return project;
        }

        public IEnumerable<Project> GetProjectsByUserID(int userID)
        {
            IEnumerable<Project> Projects = _db.Users.Single(x => x.Id == userID).Projects;

            return Projects;
        }

        public IEnumerable<ProjectFile> GetProjectFilesByProjectID(int projectID)
        {
            IEnumerable<ProjectFile> files = _db.Projects.Single(x => x.ID == projectID).ProjectFiles;

            return files;
        }

        public ProjectFile GetProjectFileByID(int projectID, int projectFileID)
        {
            return _db.Projects.Single(x => x.ID == projectID).ProjectFiles.Single(y => y.ID == projectFileID);
        }

        
    }
}