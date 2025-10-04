using chatgroup_server.Interfaces.IServices;
using FirebaseAdmin.Messaging;

namespace chatgroup_server.Services
{
    public class FirebaseService : IFirebaseService
    {
        public async Task<string> SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Webpush = new WebpushConfig()
                {
                    Notification = new WebpushNotification()
                    {
                        Title = title,
                        Body = body,
                        Icon = "/logo192.png"
                    }
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
