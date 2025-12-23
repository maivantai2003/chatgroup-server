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
        public async Task<string> SendToTokenAsync(string token,string title,string body,string action, Dictionary<string, string>? data = null)
        {
            var message = BuildMessage(title, body, action, data);
            message.Token = token;
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<List<string>> SendMulticastAsync(IEnumerable<string> tokens,string title,string body,string action,Dictionary<string, string>? data = null){
            var invalidTokens = new List<string>();

            var messages = tokens
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .Select(token => new Message
                {
                    Token = token,
                    Data = BuildData(title, body, action, data),
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    }
                })
                .ToList();

            var response = await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);

            for (int i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                {
                    invalidTokens.Add(messages[i].Token);
                }
            }

            return invalidTokens;
        }
        public async Task SendToTopicAsync(string topic,string title, string body,string action,  Dictionary<string, string>? data = null){
            var message = new Message
            {
                Topic = topic,
                Data = BuildData(title, body, action, data),
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        private static Message BuildMessage(string title, string body,string action,Dictionary<string, string>? data){
            return new Message
            {
                Data = BuildData(title, body, action, data),
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Webpush = new WebpushConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        ["Urgency"] = "high"
                    }
                }
            };
        }

        private static Dictionary<string, string> BuildData(string title,string body,string action,Dictionary<string, string>? data){
            var result = new Dictionary<string, string>
            {
                ["title"] = title,
                ["body"] = body,
                ["action"] = action
            };

            if (data != null)
            {
                foreach (var kv in data)
                    result[kv.Key] = kv.Value;
            }

            return result;
        }

        public Task RemoveInvalidTokensAsync(IEnumerable<string> tokens)
        {
            throw new NotImplementedException();
        }
    }
}

