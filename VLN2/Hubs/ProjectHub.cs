using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using VLN2.Models;
using VLN2.Services;

namespace VLN2.Hubs
{
    public class ProjectHubHelper
    {
        public static string GetLobbyName(int ProjectID, int ProjectFileID)
        {
            return ProjectID + "-" + ProjectFileID;
        }
    }

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
        ProjectsService _service = new ProjectsService();

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

            Clients.Caller.openFile(data);
        }

        public void InsertCode(int projectID, int projectFileID, string row, string column, string value)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);

            // Modify ProjectFileSession
            var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].CurrentlyOpenedFile;
            List<string> lines = projectFile.Content.Split('\n').ToList();
            string result = "";
            int tRow = int.Parse(row);
            int tCol = int.Parse(column);

            if (tRow > lines.Count)
            {
                lines.Add(value);
            }
            else
            {
                result += lines[tRow].Substring(0, tCol);
                result += value;
                result += lines[tRow].Substring(tCol);
                lines[tRow] = result;
            }
            projectFile.Content = String.Join("\n", lines.ToArray());

            Clients.OthersInGroup(projectFileLobbyName).insertCode(row, column, value);
        }

        public void RemoveCode(int projectID, int projectFileID, string row, string column, string endrow, string endcolumn)
        {
            string projectFileLobbyName = ProjectHubHelper.GetLobbyName(projectID, projectFileID);

            // Modify ProjectFileSession
            var projectFile = ProjectFileSessionsByLobbyName[projectFileLobbyName].CurrentlyOpenedFile;
            List<string> lines = projectFile.Content.Split('\n').ToList();
            string result = "";
            int tRow = int.Parse(row);
            int tCol = int.Parse(column);
            int tEndCol = int.Parse(endcolumn);
            int tEndRow = int.Parse(endrow);

            string temp = projectFile.Content;
            int toIndexFirst = 0;
            if (tRow != 0)
            {
                toIndexFirst = IndexOfOccurence(temp, "\n", tRow) + 1;
            }
            result += temp.Substring(0, toIndexFirst  + tCol);

            int fromIndex = 0;
            if (tEndRow != 0)
            {
                fromIndex = IndexOfOccurence(temp, "\n", tEndRow) + 1;
            }

            result += temp.Substring(fromIndex + tEndCol);

            projectFile.Content = result;
            var db = new ApplicationDbContext();
            var file = db.Projects.Single(x => x.ID == projectID).ProjectFiles.Single(y => y.ID == projectFileID);
            file.Content = projectFile.Content;
            db.SaveChanges();

            Clients.OthersInGroup(projectFileLobbyName).removeCode(row, column, endrow, endcolumn);
        }

        public void AddFile(string lobbyName, string filename)
        {
            Clients.Group(lobbyName).newFileAdded(filename);
        }

        public void RemoveFile(string lobbyName, string filename)
        {
            Clients.Group(lobbyName).fileRemoved(filename);
        }

        private int IndexOfOccurence(string s, string match, int occurence)
        {
            int i = 1;
            int index = 0;

            while (i <= occurence && (index = s.IndexOf(match, index + 1)) != -1)
            {
                if (i == occurence)
                    return index;

                i++;
            }

            return -1;
        }

    }
}