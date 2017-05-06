using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using VLN2.Models;
using VLN2.Services;

namespace VLN2.Hubs
{
    public class ProjectSession
    {
        public string OpenedFile { get; set; }
    }

    public class ProjectHub : ChatHub
    {
        ProjectsService _service = new ProjectsService();

        public static Dictionary<string, ProjectSession> ProjectSessions = new Dictionary<string, ProjectSession>();

        public void RequestFile(int projectID, int projectFileID)
        {
            var file = _service.GetProjectFileByID(projectID, projectFileID);
            var data = new JavaScriptSerializer().Serialize(new {
                file.ID,
                file.Name,
                file.Content
            });

            Clients.Caller.openFile(data);
        }

        public void InsertCode(string lobbyName, string row, string column, string value)
        {
            Clients.OthersInGroup(lobbyName).insertCode(row, column, value);
        }

        public void RemoveCode(string lobbyName, string row, string column, string endrow, string endcolumn)
        {
            Clients.OthersInGroup(lobbyName).removeCode(row, column, endrow, endcolumn);
        }

        public void AddFile(string lobbyName, string filename)
        {
            Clients.Group(lobbyName).newFileAdded(filename);
        }

        public void RemoveFile(string lobbyName, string filename)
        {
            Clients.Group(lobbyName).fileRemoved(filename);
        }

    }
}