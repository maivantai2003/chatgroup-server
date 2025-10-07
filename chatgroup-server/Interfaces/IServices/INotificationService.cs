namespace chatgroup_server.Interfaces.IServices
{
    public interface INotificationService
    {
        string[] getFcmToken(int userId);
    }
}
