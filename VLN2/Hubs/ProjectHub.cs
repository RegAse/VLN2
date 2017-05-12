using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using VLN2.Models;
using VLN2.Services;

namespace VLN2.Hubs
{
    public class ProjectFileSession
    {
        public ProjectFile ProjectFile { get; set; }

        public ProjectFileSession(ProjectFile projectFile)
        {
            ProjectFile = projectFile;
        }
    }

    public class ProjectHub : ChatHub
    {
        ProjectService _service = new ProjectService();
        ProgrammingLanguageService _serviceLanguage = new ProgrammingLanguageService();

        public static Dictionary<string, string> FileLobbyNameByConnection = new Dictionary<string, string>();
        public static Dictionary<string, ProjectFileSession> ProjectFileSessionsByLobbyName = new Dictionary<string, ProjectFileSession>();

        /// <summary>
        /// Gets called when a user wants to open a file from a project.
        /// </summary>
        /// <param name="projectID">The ID of the project</param>
        /// <param name="projectFileID">The ID of the file</param>
        public void RequestFile(int projectID, int projectFileID)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);

            // Create session for file if none exists.
            if(!ProjectFileSessionsByLobbyName.ContainsKey(projectFileLobbyName))
            {
                var file = _service.GetProjectFileByID(projectID, projectFileID);

                ProjectFileSessionsByLobbyName.Add(projectFileLobbyName, new ProjectFileSession(file));
            }

            // Remove user from previous lobby
            if (FileLobbyNameByConnection.ContainsKey(Context.ConnectionId))
            {
                Groups.Remove(Context.ConnectionId, FileLobbyNameByConnection[Context.ConnectionId]);
                FileLobbyNameByConnection.Remove(Context.ConnectionId);
            }

            // Add user to new file lobby
            Groups.Add(Context.ConnectionId, projectFileLobbyName);
            FileLobbyNameByConnection.Add(Context.ConnectionId, projectFileLobbyName);

            var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].ProjectFile;
            var data = new JavaScriptSerializer().Serialize(new {
                projectFile.ID,
                projectFile.Name,
                projectFile.Content
            });

            string extension = System.IO.Path.GetExtension(projectFile.Name).Substring(1);
            ProgrammingLanguage lang = _serviceLanguage.GetProgrammingLanguageByExtension(extension);
            string mode = "text";
            if (lang != null)
            {
                mode = lang.Name.ToLower();
            }

            Clients.Caller.openFile(data, mode);
        }

        /// <summary>
        /// Gets called when a user makes a change to a file
        /// </summary>
        /// <param name="projectID">The ID of the project</param>
        /// <param name="projectFileID">The ID of the file</param>
        /// <param name="obj">The changes made</param>
        public void FileChanged(int projectID, int projectFileID, string obj)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);
            Clients.OthersInGroup(projectFileLobbyName).fileChanged(obj);
        }

        /// <summary>
        /// Gets called when a user wants to save the file.
        /// </summary>
        /// <param name="projectID">The ID of the project</param>
        /// <param name="projectFileID">The ID of the file</param>
        /// <param name="obj">The entire file to be saved</param>
        public void SaveFile(int projectID, int projectFileID, string obj)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);
            if (ProjectFileSessionsByLobbyName.ContainsKey(projectFileLobbyName))
            {
                var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].ProjectFile;

                var db = new ApplicationDbContext();
                var file = db.Projects.Single(x => x.ID == projectID).ProjectFiles.Single(y => y.ID == projectFileID);
                projectFile.Content = obj; // Start by setting the session value so the server doesn't need to wait for the database to reply
                file.Content = obj;
                db.SaveChanges();

                Clients.Caller.changesSaved();
            }
        }

        /// <summary>
        /// Gets called when a user want to add a new file,
        /// let's all other users in this project know, but 
        /// only calls the caller if a file already exists 
        /// with the same name.
        /// </summary>
        /// <param name="lobbyName">The ID of the project</param>
        /// <param name="filename">The name of the new file</param>
        public void AddFile(string lobbyName, string filename)
        {
            int projectID = int.Parse(lobbyName);
            // Create the file

            var db = new ApplicationDbContext();
            var file = db.ProjectFiles.SingleOrDefault(x => (x.Name == filename && x.Project.ID == projectID));

            if (file == null)
            {
                // Create the file
                var projectFile = new ProjectFile();
                projectFile.Name = filename;
                projectFile.Content = "";

                var project = db.Projects.Single(x => x.ID == projectID);
                project.ProjectFiles.Add(projectFile);
                db.SaveChanges();

                Clients.Group(lobbyName).newFileAdded(projectFile.ID, filename);
            }
            else
            {
                string message = "A file already exists with name: " + filename;
                Clients.Caller.newfileAddedFailed(message);
            }
        }

        /// <summary>
        /// Gets called when a user want to remove a file.
        /// </summary>
        /// <param name="lobbyName">The ID of the project</param>
        /// <param name="fileID">The ID of the file</param>
        public void RemoveFile(string lobbyName, string fileID)
        {
            int projectFileID = int.Parse(fileID);

            _service.RemoveProjectFile(projectFileID);

            Clients.Group(lobbyName).fileRemoved(fileID);
        }

        /// <summary>
        /// Gets called when a user moves his cursor inside a file, 
        /// It will let all other users that have this file open 
        /// know of the the location of the cursor.
        /// </summary>
        /// <param name="projectID">The ID of the project</param>
        /// <param name="projectFileID">The ID of the project file</param>
        /// <param name="cursorData">The location of the string</param>
        public void CursorPositionChanged(int projectID, int projectFileID, string cursorData)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);

            if(UserLobbies.ContainsKey(projectID.ToString()))
            {
                ChatUser user = UserLobbies[projectID.ToString()].Users.Single(x => x.ConnectionID == Context.ConnectionId);
                user.CustomData = cursorData;

                string userData = Json.Encode(user);
                Clients.OthersInGroup(projectFileLobbyName).cursorMoved(userData);
            }
        }

    }
}