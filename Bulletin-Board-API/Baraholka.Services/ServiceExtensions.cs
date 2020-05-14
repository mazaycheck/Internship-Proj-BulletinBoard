using System.Security.Claims;

namespace Baraholka.Services
{
    public static class ServiceExtensions
    {
        public static int GetUserID(this ClaimsPrincipal User)
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}