using chatgroup_server.Dtos;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Models;
using chatgroup_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
using tryAGI.OpenAI;
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
        //Add member in group
        public async Task AddMemberToGroup(string userId,Conversation conversation)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("MemberToGroup",conversation);
            }
        }
        // Join group
        //Quân có 2 group Group A:1, Group B:2, khi nhấn vào group A thì sẽ tạo kết nối nhóm A với Quân, group B cũng tương tự
        public async Task JoinGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            await Clients.Group(groupId).SendAsync("UserJoin", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value+","+groupId);
        }
        public async Task LeaveGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
            await Clients.Group(groupId).SendAsync("UserLeave", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value + "," + groupId);
        }
        //Quân có 2 group Group A:1, Group B:2, khi nhấn vào group A thì sẽ tạo kết nối nhóm A với Quân thì lúc này Quân gửi tin nhắn trong group A được
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
        //Typing when sender send message
        public async Task HoverSendUserMessage(string userId, string message) {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveHoverUserMessage",userId,message);
            }
        }
        //Typing when sender send message to group
        public async Task HoverSendGroupMessage(string groupId,string userId, string message)
        {
            await Clients.Group(groupId).SendAsync("ReceiveHoverGroupMessage", userId,groupId,message);
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
            await Clients.Group(groupId).SendAsync("ReceiveGroupMessageFileInfor", groupId,userId, listFile);
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
        //cancel friend
        public async Task CancelFriend(string userId)
        {
            var connections = _connection.GetUserConections(userId);
            foreach (var connectionId in connections)
            {
               await Clients.Client(connectionId).SendAsync("ReceiveCancelFriend");
            }
        }
        //cancel group
        public async Task CancelGroup(string groupId,string userId)
        {
            await Clients.Group(groupId).SendAsync("ReceiveCancelGroup");
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connection.RemoveUserConnection(Context.ConnectionId);
            Console.WriteLine($"User disconnected: {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }
        //Call video
        public async Task CallUser(string targetUserId)
        {
            var connections = _connection.GetUserConections(targetUserId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveCall", connectionId);
            }
        }
        //Accept Call
        public async Task AnswerCall(string callerUserId)
        {
            var connections = _connection.GetUserConections(callerUserId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("CallAccepted");
            }
            
        }
        //Reject Call
        public async Task RejectCall(string toUserId)
        {
            var connections = _connection.GetUserConections(toUserId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    await Clients.Client(connectionId).SendAsync("CallRejected", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
                }
            }
        }
        //
        // Gửi lời mời gọi video
        public async Task SendCallRequest(string toUserId)
        {
            var connections = _connection.GetUserConections(toUserId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    //await Clients.Client(connectionId).SendAsync("CallRejected");
                    await Clients.Client(connectionId).SendAsync("ReceiveCallRequest", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
                }
                
            }
        }
        // Gửi offer SDP
        public async Task SendOffer(string toUserId, string offer)
        {
            var connections = _connection.GetUserConections(toUserId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    //await Clients.Client(connectionId).SendAsync("CallRejected");
                    await Clients.Client(connectionId).SendAsync("ReceiveOffer", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, offer);
                }
                
            }
        }
        // Gửi answer SDP
        public async Task SendAnswer(string toUserId, string answer)
        {
            var connections = _connection.GetUserConections(toUserId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    //await Clients.Client(connectionId).SendAsync("CallRejected");
                    await Clients.Client(connectionId).SendAsync("ReceiveAnswer", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, answer);
                }
                
            }
        }

        // Gửi ICE Candidate
        public async Task SendIceCandidate(string toUserId, string candidate)
        {
            var connections = _connection.GetUserConections(toUserId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    //await Clients.Client(connectionId).SendAsync("CallRejected");
                    await Clients.Client(connectionId).SendAsync("ReceiveIceCandidate", Context.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, candidate);
                }
                
            }
        }
    }
}
