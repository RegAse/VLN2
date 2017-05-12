using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLN2.Models;

namespace VLN2.Tests.ServiceClasses
{
    public class ProjectService
    {

        private IAppDataContext _db;

        public ProjectService(IAppDataContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Get project by ID.
        /// </summary>
        /// <param name="projectID">The ID of a project</param>
        /// <returns>The project associated with the ID</returns>
        public Project GetProjectByID(int projectID)
        {
            return _db.Projects.SingleOrDefault(x => x.ID == projectID);
        }

        /*/// <summary>
        /// Get all projects a user has access to.
        /// </summary>
        /// <param name="userID">The ID of a user</param>
        /// <returns>A collection of projects</returns>
        public IEnumerable<Project> GetProjectsByUserID(int userID)
        {
            return _db.Users.Single(x => x.Id == userID).UserHasProjects.Select(pr => pr.Project);
        }*/

        /// <summary>
        /// Get all project files of a project.
        /// </summary>
        /// <param name="projectID">The ID of a project</param>
        /// <returns>A collection of ProjectFile</returns>
        public IEnumerable<ProjectFile> GetProjectFilesByProjectID(int projectID)
        {
            return _db.Projects.Single(x => x.ID == projectID).ProjectFiles;
        }

        /// <summary>
        /// Get a project file by ID.
        /// </summary>
        /// <param name="projectFileID">The ID of the ProjectFile</param>
        /// <returns>A project</returns>
        public ProjectFile GetProjectFileByID(int projectFileID)
        {
            return _db.ProjectFiles.Single(y => y.ID == projectFileID);
        }

        /// <summary>
        /// Remove project file by ID.
        /// </summary>
        /// <param name="projectFileID">The ID of the project</param>
        public void RemoveProjectFile(int projectFileID)
        {
            var projectFile = _db.ProjectFiles.Single(x => x.ID == projectFileID);
            _db.ProjectFiles.Remove(projectFile);
            _db.SaveChanges();
        }

        /// <summary>
        /// Removes a collaborator from a project.
        /// </summary>
        /// <param name="projectID">The ID of the Project</param>
        /// <param name="userID">The ID of the user</param>
        public void RemoveCollaborator(int projectID, int userID)
        {
            var userHasProject = _db.UserHasProject.Single(x => (x.ProjectID == projectID) && (x.UserID == userID));
            _db.UserHasProject.Remove(userHasProject);
            _db.SaveChanges();
        }

    }
}
