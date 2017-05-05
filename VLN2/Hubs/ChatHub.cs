﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace VLN2.Hubs
{
    public class ChatUser
    {
        public string Name { get; set; }
        public string ConnectionID { get; set; }

        public ChatUser(string name, string connectionId)
        {
            Name = name;
            ConnectionID = connectionId;
        }
    }

    public class ChatHub : Hub
    {
        public static Dictionary<string, string> LobbyNameByConnection = new Dictionary<string, string>();
        public static Dictionary<string, int> NumberOfConnectedUsersInLobby = new Dictionary<string, int>();

        public async Task JoinLobby(string lobbyName)
        {
            await Groups.Add(Context.ConnectionId, lobbyName);

            string name = Context.User.Identity.Name;

            if (NumberOfConnectedUsersInLobby.ContainsKey(lobbyName))
            {
                NumberOfConnectedUsersInLobby[lobbyName] += 1;
            }
            else
            {
                NumberOfConnectedUsersInLobby[lobbyName] = 1;
            }
            // Add the connection to a dictionary to be able to find it later with connection id.
            LobbyNameByConnection.Add(Context.ConnectionId, lobbyName);

            // Let others know that someone joined
            Clients.OthersInGroup(lobbyName).userJoinedLobby(name);
            
            // Let the caller know now
            Clients.Caller.joined(NumberOfConnectedUsersInLobby[lobbyName]);
        }

        public Task LeaveLobby(string lobbyName)
        {
            NumberOfConnectedUsersInLobby[lobbyName] -= 1;
            return Groups.Remove(Context.ConnectionId, lobbyName);
        }

        public void Send(string lobbyName, string message)
        {
            string name = Context.User.Identity.Name;

            // Call the addNewMessageToPage method to update clients.
            Clients.Group(lobbyName).addNewMessageToPage(name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            if (LobbyNameByConnection.ContainsKey(Context.ConnectionId))
            {
                string lobbyName = LobbyNameByConnection[Context.ConnectionId];
                NumberOfConnectedUsersInLobby[lobbyName] -= 1;
                Clients.Group(lobbyName).userLeftLobby(name);
            }

            return base.OnDisconnected(stopCalled);
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