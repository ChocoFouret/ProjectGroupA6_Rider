using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
namespace WebSocketDemo.Hubs
{
    public class EventsHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }

    public struct WebSocketActions
    {
        public static readonly string MESSAGE_UPDATED = "updated";
        public static readonly string MESSAGE_DELETED = "deleted";
        public static readonly string MESSAGE_CREATED = "created";
    }
}