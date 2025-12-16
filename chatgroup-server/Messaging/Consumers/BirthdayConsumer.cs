using chatgroup_server.Hubs;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Messaging.Contracts;
using chatgroup_server.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace chatgroup_server.Messaging.Consumers
{
    public class BirthdayConsumer(IConversationService _conversation,IHubContext<myHub> _hub) : IConsumer<BirthdayMessage>
    {
        public async Task Consume(ConsumeContext<BirthdayMessage> context)
        {
            var data = context.Message;

            var conversation = new Conversation
            {
                UserId = data.SenderId,
                Id = data.FriendId,
                Type = "user",
                UserSend = data.UserName,
                Content = $"🎉 Hôm nay là sinh nhật của {data.UserName}!"
            };

            await _hub.Clients
                .User(data.FriendId.ToString())
                .SendAsync("birthday", conversation);
        }
    }
}
