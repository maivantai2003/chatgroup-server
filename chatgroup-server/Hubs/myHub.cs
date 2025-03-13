using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Services;
using Microsoft.AspNetCore.SignalR;

namespace chatgroup_server.Hubs
{
    public class myHub : Hub
    {
        private readonly IManagerConection _connection;
        public myHub(IManagerConection connection)
        {
            _connection = connection;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _connection.AddUserConnection(userId, Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }
        public async Task AcceptFriend(string userId)
        {
            var connections=_connection.GetUserConections(userId);
            if (connections == null || !connections.Any()) return;
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveAcceptFriend");
            }
        }
        public async Task AddMemberToGroup(string userId)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("MemberToGroup");
            }
        }
        public async Task JoinGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }
        public async Task SendGroupMesage(string groupId)
        {

            await Clients.Group(groupId).SendAsync("ReceiveGroupMessage");
        }
        public async Task SendUserMessage(string userId)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveUserMessage");
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connection.RemoveUserConnection(Context.ConnectionId);
            Console.WriteLine($"🔴 User disconnected: {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
