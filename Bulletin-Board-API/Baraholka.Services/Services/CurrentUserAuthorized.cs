using System.Security.Claims;

namespace Baraholka.Services.Services
{
    public class CurrentContextUserAuthorized : ICurrentUserAuthorized
    {
        public int UserId => int.Parse(ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}