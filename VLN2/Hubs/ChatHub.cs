﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using VLN2.Extensions;
using System.Web.Script.Serialization;
using System.Web.Helpers;
using VLN2.Attributes;

namespace VLN2.Hubs
{
    public class ChatUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ConnectionID { get; set; }

        public string MarkerClass { get; set; }
        public string CustomData { get; set; }

        public ChatUser(int id, string username, string displayName, string connectionId)
        {
            ID = id;
            Username = username;
            DisplayName = displayName;
            ConnectionID = connectionId;
        }
    }

    public class UserLobby
    {
        public List<ChatUser> Users { get; set; }
        private int MarkerClassPool { get; set; }

        public UserLobby()
        {
            Users = new List<ChatUser>();
            MarkerClassPool = 0;
        }

        public int GetNextMarker()
        {
            MarkerClassPool++;
            return MarkerClassPool % 10;
        }
    }

    [Authorize]
    [ProjectAccess(AccessLevel = "Normal User")]
    public class ChatHub : Hub
    {
        public static Dictionary<string, string> LobbyNameByConnection = new Dictionary<string, string>();
        public static Dictionary<string, int> NumberOfConnectedUsersInLobby = new Dictionary<string, int>();

        // List
        public static Dictionary<string, UserLobby> UserLobbies = new Dictionary<string, UserLobby>();

        /// <summary>
        /// A user joined the lobby.
        /// </summary>
        /// <param name="lobbyName">the identifier for the lobby</param>
        /// <returns></returns>
        public virtual async Task JoinLobby(string lobbyName)
        {
            await Groups.Add(Context.ConnectionId, lobbyName);

            int id = Context.User.Identity.GetUserId<int>();
            string name = Context.User.Identity.Name;
            string displayname = Context.User.Identity.GetDisplayname();
            string connectionID = Context.ConnectionId;

            // Create the list of users if this is the first person to join the hub
            if (!UserLobbies.ContainsKey(lobbyName))
            {
                UserLobby lobby = new UserLobby();

                UserLobbies.Add(lobbyName, lobby);
            }

            ChatUser user = new ChatUser(id, name, displayname, connectionID);

            // set unique-ish color to the the user
            string markerClassName = "user-color-" + UserLobbies[lobbyName].GetNextMarker();
            user.MarkerClass = markerClassName;
            
            // Add user to lobby
            UserLobbies[lobbyName].Users.Add(user);

            // Add the connection to a dictionary to be able to find it later with connection id.
            LobbyNameByConnection.Add(Context.ConnectionId, lobbyName);

            string userData = Json.Encode(user);

            // Let others know that someone joined
            Clients.OthersInGroup(lobbyName).userJoinedLobby(userData);

            string usersData = Json.Encode(UserLobbies[lobbyName].Users);
            // Let the caller know now
            Clients.Caller.joined(usersData);
        }

        /// <summary>
        /// A user left the lobby.
        /// </summary>
        /// <param name="lobbyName">the identifier for the lobby</param>
        /// <returns></returns>
        public Task LeaveLobby(string lobbyName)
        {
            NumberOfConnectedUsersInLobby[lobbyName] -= 1;
            return Groups.Remove(Context.ConnectionId, lobbyName);
        }

        public void Send(string lobbyName, string message)
        {
            string name = Context.User.Identity.Name;

            // Call the addNewMessageToPage method to update clients.
            Clients.Group(lobbyName).addNewMessage(name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            int id = Context.User.Identity.GetUserId<int>();
            string name = Context.User.Identity.Name;

            // Make sure the connection actually belongs to some user inside a lobby
            if (LobbyNameByConnection.ContainsKey(Context.ConnectionId))
            {
                string lobbyName = LobbyNameByConnection[Context.ConnectionId];

                // Remove the user from the lobby
                if (UserLobbies.ContainsKey(lobbyName))
                {
                    ChatUser user = UserLobbies[lobbyName].Users.Where(x => x.ConnectionID == Context.ConnectionId).First();
                    string displayname = user.DisplayName;
                    int userID = user.ID;
                    UserLobbies[lobbyName].Users.Remove(user);

                    Clients.Group(lobbyName).userLeftLobby(Context.ConnectionId, displayname);
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}