using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Hubs
{
    public class ProjectHub : ChatHub
    {

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