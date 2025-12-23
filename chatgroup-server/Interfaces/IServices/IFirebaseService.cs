namespace chatgroup_server.Interfaces.IServices
{
    public interface IFirebaseService
    {
        Task<string> SendNotificationAsync(string token, string title, string body);
        Task<string> SendToTokenAsync(string token, string title, string body,string action,Dictionary<string, string>? data = null);
        Task<List<string>> SendMulticastAsync(IEnumerable<string> tokens,string title,string body,string action,Dictionary<string, string>? data = null);
        Task SendToTopicAsync(string topic,string title,string action,string body,Dictionary<string, string>? data = null);
        Task RemoveInvalidTokensAsync(IEnumerable<string> tokens);
    }
}
