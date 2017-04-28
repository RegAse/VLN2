using System;
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
        public static Dictionary<string, ChatUser> ConnectedUsers = new Dictionary<string, ChatUser>();

        public void Hello()
        {
            string name = Context.User.Identity.Name;
            string id = Context.ConnectionId;

            if (!ConnectedUsers.ContainsKey(id))
            {
                ConnectedUsers.Add(id, new ChatUser(name, id));

                Clients.All.userJoinedLobby(name);
            }
            System.Diagnostics.Debug.WriteLine("COUNT OF: " + ConnectedUsers.Count);
            Clients.Caller.joined(ConnectedUsers.Count);
        }

        public void Send(string message)
        {
            string name = Context.User.Identity.Name;

            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            ConnectedUsers.Remove(Context.ConnectionId);

            Clients.All.userLeftLobby(name);
            return base.OnDisconnected(stopCalled);
        }

    }
}