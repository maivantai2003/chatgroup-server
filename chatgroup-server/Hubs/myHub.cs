using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace chatgroup_server.Hubs
{
    //[Authorize]
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
                await Clients.All.SendAsync("CheckConnection", _connection.GetAllConnectedUsers());
            }
            await base.OnConnectedAsync();
        }
        public async Task AcceptFriend(string userId,Conversation conversation)
        {
            var connections=_connection.GetUserConections(userId);
            if (connections == null || !connections.Any()) return;
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveAcceptFriend",conversation);
            }
        }
        public async Task AddMemberToGroup(string userId,Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("MemberToGroup",conversation);
            }
        }
        public async Task JoinGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            await Clients.Group(groupId).SendAsync("UserJoin", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value+","+groupId);
        }
        public async Task SendGroupMessage(string groupId,string userId,GroupMessageResponseDto groupMessage)
        {

            await Clients.Group(groupId).SendAsync("ReceiveGroupMessage",userId,groupMessage);
        }
        public async Task SendConversationGroup(string userId, ConversationUpdateTimeDto conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("UpdateConversationGroup", conversation);
            }
        }
        public async Task SendUserMessage(string userId,UserMessageResponseDto userMessage,Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveUserMessage", userMessage);
                await Clients.Client(connectionId).SendAsync("UpdateConversationUser", conversation);
                await Clients.Client(connectionId).SendAsync("CheckUser",_connection.GetAllConnectedUsers());
            }
            
        }
        public async Task SendCloudMessage(string userId, CloudMessageResponseDto cloudMessage, Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveCloudMessage", cloudMessage);
                await Clients.Client(connectionId).SendAsync("UpdateConversationCloud", conversation);
                await Clients.Client(connectionId).SendAsync("CheckUser", _connection.GetAllConnectedUsers());
            }

        }
        public async Task HoverSendUserMessage(string userId, string message) {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveHoverUserMessage",message);
            }
        }
        public async Task HoverSendGroupMessage(string groupId,string userId, string message)
        {
            await Clients.Group(groupId).SendAsync("ReceiveHoverGroupMessage", userId,message);
        }
        public async Task AwaitProgressSendFile(string userId,double progress)
        {
            var connections=_connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ProgressSendFile", progress);
            }
        }
        public async Task LoadRequestFriend(string userId)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("RequestFriend",userId);
            }
        }
        public async Task SendRequestFriend(string userId)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("RequestFriend", userId);
            }
        }
        //
        public async Task SendCloudMessageFile(string userId,CloudMessageFileSendDto file)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveCloudMessageFile", file);
            }
        }
        //
        public async Task SendUserMessageFile(string userId,string senderId, UserMessageFileSendDto file,IEnumerable<UserMessageFileResponseDto> listFile)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveUserMessageFile", file);
                await Clients.Client(connectionId).SendAsync("ReceiveUserMessageFileInfor",senderId, listFile);
            }
        }
        //
        public async Task SendGroupMessageFile(string groupId, string userId, GroupMessageFileSendDto file, IEnumerable<GroupMessageFileResponseDto> listFile)
        {
            await Clients.Group(groupId).SendAsync("ReceiveGroupMessageFile", userId, file);
            await Clients.Group(groupId).SendAsync("ReceiveGroupMessageFileInfor", userId, listFile);
        }
        public async Task AddConversationMemberGroup(string userId,Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveConversationMemberGroup", conversation);
            }
        }
        public async Task UpdateConversationMemberInGroup(string userId, Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveConversationMemberInGroup", conversation);
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
