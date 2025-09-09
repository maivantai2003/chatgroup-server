using System.Security.Claims;

namespace chatgroup_server.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdStr=user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr,out var userId)?userId:0;
        }
    }
}
