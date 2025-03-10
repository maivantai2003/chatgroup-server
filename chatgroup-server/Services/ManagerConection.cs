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

        public int GetUserOnline()
        {
            return _userConnections.Count;
        }

        public void RemoveUserConnection(string connectionId)
        {
            foreach (var user in _userConnections.Keys)
            {
                if (_userConnections.TryGetValue(user, out var connections) && connections.Contains(connectionId))
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _userConnections.TryRemove(user, out _);
                    }
                    break;
                }
            }
        }
    }
}
