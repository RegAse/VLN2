using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Hubs
{
    public class ProjectSession
    {
        public string OpenedFile { get; set; }
    }

    public class ProjectHub : ChatHub
    {
        public static Dictionary<string, ProjectSession> ProjectSessions = new Dictionary<string, ProjectSession>();

        public void RequestFile(string lobbyName, string filename)
        {
            string fileContent = "var = 23;";
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