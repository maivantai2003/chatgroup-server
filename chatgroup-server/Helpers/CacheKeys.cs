namespace chatgroup_server.Helpers
{
    public static class CacheKeys
    {
        public static string Friends(int userId) => $"friends:{userId}";
        public static string User(int userId) => $"user:{userId}";
        public static string Users(int userId) => $"users:{userId}";
        public static string Groups(int groupId) => $"groups:{groupId}";
        public static string GroupsOfUser(int userId) => $"groups:user-{userId}";
        public static int Time = 20;
    }
}
