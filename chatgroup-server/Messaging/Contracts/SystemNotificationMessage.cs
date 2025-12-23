using chatgroup_server.Common;

namespace chatgroup_server.Messaging.Contracts
{
    public class SystemNotificationMessage
    {
        public NotificationTarget Target { get; set; }
        public int? UserId { get; set; }
        public List<int>? UserIds { get; set; }
        public string? Topic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Action { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}
