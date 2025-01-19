using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;

namespace ECommerceSkinet.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public Address Address { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}
