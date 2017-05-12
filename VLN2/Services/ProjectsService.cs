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
            return _db.Projects.SingleOrDefault(x => x.ID == projectID);
        }

        public IEnumerable<Project> GetProjectsByUserID(int userID)
        {
            return _db.Users.Single(x => x.Id == userID).UserHasProjects.Select(pr => pr.Project);
        }

        /// <summary>
        /// Get all project files of a project
        /// </summary>
        /// <param name="projectID">The ID of a project</param>
        /// <returns>All project files associated with the projectID</returns>
        public IEnumerable<ProjectFile> GetProjectFilesByProjectID(int projectID)
        {
            return _db.Projects.Single(x => x.ID == projectID).ProjectFiles;
        }

        public ProjectFile GetProjectFileByID(int projectID, int projectFileID)
        {
            return _db.Projects.Single(x => x.ID == projectID).ProjectFiles.Single(y => y.ID == projectFileID);
        }

        
    }
}