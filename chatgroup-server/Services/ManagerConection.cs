using chatgroup_server.Interfaces.IServices;
using System.Collections.Concurrent;

namespace chatgroup_server.Services
{
    public class ManagerConection : IManagerConection
    {
        private static ConcurrentDictionary<string, HashSet<string>> _userConnections = new();

        public void AddUserConnection(string userId, string connectionId)
        {
            _userConnections.AddOrUpdate(
                userId,
                new HashSet<string> { connectionId },
                (_, existingConnections) =>
                {
                    existingConnections.Add(connectionId);
                    return existingConnections;
                });
        }

        public List<string> GetUserConections(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connections) ? connections.ToList() : new List<string>();
        }

        //public List<string> GetUserConnections(string userId)
        //{
        //    return _userConnections.TryGetValue(userId, out var connections) ? connections.ToList() : new List<string>();
        //}
        public List<string> GetAllConnectedUsers()
        {
            return _userConnections.Keys.ToList();
        }
        public int GetUserOnline()
        {
            return _userConnections.Count;
        }

        public void RemoveUserConnection(string connectionId)
        {
            foreach (var kvp in _userConnections)
            {
                if (kvp.Value.Contains(connectionId))
                {
                    kvp.Value.Remove(connectionId);
                    if (kvp.Value.Count == 0) 
                    {
                        _userConnections.TryRemove(kvp.Key, out _);
                    }
                    break;
                }
            }
        }
    }
}
