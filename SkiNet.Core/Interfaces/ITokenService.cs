using ECommerceSkinet.Core.Entities.Identity;


namespace ECommerceSkinet.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
