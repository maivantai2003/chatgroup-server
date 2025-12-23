using chatgroup_server.Common;
using chatgroup_server.Interfaces.IServices;
using chatgroup_server.Messaging.Contracts;
using MassTransit;

namespace chatgroup_server.Messaging.Consumers
{
    public class SystemNotificationConsumer(IFirebaseService _firebaseService,IUserDeviceService _device) : IConsumer<SystemNotificationMessage>
    {
        public async Task Consume(ConsumeContext<SystemNotificationMessage> context)
        {
            var msg=context.Message;
            switch (msg.Target)
            {
                case NotificationTarget.SingleUser:
                    await SendToSingle(msg);
                    break;

                case NotificationTarget.MultiUser:
                    await SendToMany(msg);
                    break;

                case NotificationTarget.Topic:
                    await SendToTopic(msg);
                    break;
            }
        }
        private async Task SendToSingle(SystemNotificationMessage msg)
        {
            var tokens = await _device.GetFcmTokensByUserIdAsync(msg.UserId!.Value);
            await SendTokens(tokens, msg);
        }

        private async Task SendToMany(SystemNotificationMessage msg)
        {
            var tokens = await _device.GetFcmTokensByUserIdsAsync(msg.UserIds!);
            await SendTokens(tokens, msg);
        }
        private async Task SendTokens(IEnumerable<string> tokens, SystemNotificationMessage msg)
        {
            var tokenList = tokens.Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToList();
            if (!tokenList.Any()) return;

            var invalidTokens = await _firebaseService.SendMulticastAsync(
                tokenList,
                msg.Title,
                msg.Body,
                msg.Action,
                msg.Data
            );
        }
        private async Task SendToTopic(SystemNotificationMessage msg)
        {
            await _firebaseService.SendToTopicAsync(
                msg.Topic!,
                msg.Title,
                msg.Body,
                msg.Action,
                msg.Data
            );
        }
    }
}
