using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Messaging.Contracts;
using chatgroup_server.Services;
using MassTransit;

namespace chatgroup_server.Messaging.Consumers
{
    public class NotificationConsumer(IFirebaseService _firebase,IUserDeviceService _device):IConsumer<NotificationMessage>
    {
        public async Task Consume(ConsumeContext<NotificationMessage> context)
        {
            var msg = context.Message;
            //var tokens = await _device.GetFcmTokensByUserIdAsync(msg.UserId);
            var fcmTokens = new List<string>() { "cqsxBb73rsswdOpaPuUE1E:APA91bG30GrBlwleo8iM8An9kfTlWbARb3XOimc5z-jQq8S6c_UZlLy2YzeqNQvTXUAOxyp4oK6nBTCX9kYgqJ6aEfDa1LisAhFXD05KjFGToDBLfKkqn9U", "dGXc6ddVo-LqsrJ4pQczFL:APA91bF-lMtZUMAAKa2wCxJRare3rbd4SNYIhJVZkdDke1zDhetRp0FG7wZuVC1OsSP4Zu88DEkhuYL8LHY4iYNo-hyRTdBXVDAsC8pLnp9lVzzoGxRAjbo" };
            foreach (var token in fcmTokens)
            {
                await _firebase.SendNotificationAsync(token, msg.Title ?? "Thông báo", msg.Body ?? "");
            }
            foreach (var token in fcmTokens)
            {
                await _firebase.SendNotificationAsync(
                    token,
                    msg.Title ?? "Thông báo",
                    msg.Body ?? ""
                );
            }
        }
    }
}
