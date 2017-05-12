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

    public class ProjectSession
    {
        public string OpenedFile { get; set; }
    }

    public class ProjectFileSession
    {
        public ProjectFile CurrentlyOpenedFile { get; set; }

        public ProjectFileSession(ProjectFile projectFile)
        {
            CurrentlyOpenedFile = projectFile;
        }
    }

    public class ProjectHub : ChatHub
    {
        ProjectService _service = new ProjectService();
        ProgrammingLanguageService _serverLanguage = new ProgrammingLanguageService();

        //public static Dictionary<string, ProjectSession> ProjectSessions = new Dictionary<string, ProjectSession>();
        public static Dictionary<string, string> FileLobbyNameByConnection = new Dictionary<string, string>();
        public static Dictionary<string, ProjectFileSession> ProjectFileSessionsByLobbyName = new Dictionary<string, ProjectFileSession>();

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

            var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].CurrentlyOpenedFile;
            var data = new JavaScriptSerializer().Serialize(new {
                projectFile.ID,
                projectFile.Name,
                projectFile.Content
            });

            string extension = System.IO.Path.GetExtension(projectFile.Name).Substring(1);
            ProgrammingLanguage lang = _serverLanguage.GetProgrammingLanguageByExtension(extension);
            string mode = "text";
            if (lang != null)
            {
                mode = lang.Name.ToLower();
            }

            Clients.Caller.openFile(data, mode);
        }

        public void FileChanged(int projectID, int projectFileID, string obj)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);
            Clients.OthersInGroup(projectFileLobbyName).fileChanged(obj);
        }

        public void SaveFile(int projectID, int projectFileID, string obj)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);
            if (ProjectFileSessionsByLobbyName.ContainsKey(projectFileLobbyName))
            {
                var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].CurrentlyOpenedFile;

                var db = new ApplicationDbContext();
                var file = db.Projects.Single(x => x.ID == projectID).ProjectFiles.Single(y => y.ID == projectFileID);
                projectFile.Content = obj; // Start by setting the session value so the server doesn't need to wait for the database to reply
                file.Content = obj;
                db.SaveChanges();

                Clients.Caller.changesSaved();
            }
        }

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

        public void RemoveFile(string lobbyName, string fileID)
        {
            var db = new ApplicationDbContext();
            int projectID = int.Parse(lobbyName);
            int projectFileID = int.Parse(fileID);

            var project = db.Projects.Single(x => x.ID == projectID);
            var projectFile = project.ProjectFiles.Single(x => x.ID == projectFileID);
            db.ProjectFiles.Remove(projectFile);
            db.SaveChanges();

            Clients.Group(lobbyName).fileRemoved(fileID);
        }

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