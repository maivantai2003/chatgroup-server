namespace chatgroup_server.Interfaces.IServices
{
    public interface IManagerConection
    {
        void AddUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConections(string userId);
    }
}
