using ECommerceSkinet.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace ECommerceSkinet.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ali",
                    Email = "ali@test.com",
                    UserName = "ali@test.com",
                    Address = new Address
                    {
                        FirstName = "ali",
                        LastName = "ali",
                        Street = "10 street",
                        City = "cairo",
                        State = "ca",
                        PostalCode = "154274"
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
