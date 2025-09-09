using BCrypt.Net;
namespace chatgroup_server.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool Verify(string inputPassword, string storedPassword)
        {
            if (storedPassword.StartsWith("$2"))
            {
                return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
            }
            return inputPassword == storedPassword;
        }
        public static string? UpgradePasswordIfNeeded(string inputPassword, string storedPassword)
        {
            if (!storedPassword.StartsWith("$2"))
            {
                return Hash(inputPassword);
            }

            return null;
        }
    }
}
