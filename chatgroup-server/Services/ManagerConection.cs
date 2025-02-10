using chatgroup_server.Interfaces.IServices;
using System.Collections.Concurrent;

namespace chatgroup_server.Services
{
    public class ManagerConection:IManagerConection
    {
        private readonly ConcurrentDictionary<string, List<string>> _userConnections = new();
        public void AddUserConnection(string userId, string connectionId)
        {
            if (!_userConnections.ContainsKey(userId))
            {
                _userConnections[userId] = new List<string>();
            }
            _userConnections[userId].Add(connectionId);
        }

        public List<string> GetUserConections(string userId)
        {
            return _userConnections.ContainsKey(userId) ? _userConnections[userId] : new List<string>();
        }

        public void RemoveUserConnection(string connectionId)
        {
            foreach (var userConnections in _userConnections)
            {
                if (userConnections.Value.Contains(connectionId))
                {
                    userConnections.Value.Remove(connectionId);
                    if (!userConnections.Value.Any())
                    {
                        _userConnections.TryRemove(userConnections.Key, out _);
                    }
                    break;
                }
            }
        }
    }
}
