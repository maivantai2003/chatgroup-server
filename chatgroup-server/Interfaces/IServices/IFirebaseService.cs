namespace chatgroup_server.Interfaces.IServices
{
    public interface IFirebaseService
    {
        Task<string> SendNotificationAsync(string token, string title, string body);
    }
}
