using System.Security.Claims;

namespace ECommerceSkinet.WebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            //Older Syntax
            //return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}
