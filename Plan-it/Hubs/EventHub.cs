using Microsoft.AspNetCore.SignalR;
namespace WebSocketDemo.Hubs
{
    public class EventHub : Hub
    {
        /*
        static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        public async Task Register(string username)
        {
            if (Users.ContainsKey(username))
            {
                Users.Add(username, this.Context.ConnectionId);
            }

            await Clients.All.SendAsync(WebSocketActions.USER_JOINED, username);
        }

        public async Task Leave(string username)
        {
            Users.Remove(username);
            await Clients.All.SendAsync(WebSocketActions.USER_LEFT, username);
        }
        */
        public async Task Send(string username, string message)
        {
            await Clients.All.SendAsync(WebSocketActions.MESSAGE_UPDATED, username, message);
        }
    }

    public struct WebSocketActions
    {
        public static readonly string MESSAGE_UPDATED = "updated";
        public static readonly string MESSAGE_DELETED = "deleted";
        public static readonly string MESSAGE_CREATED = "created";
    }
}