using ECommerceSkinet.Core.Identity;


namespace ECommerceSkinet.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
