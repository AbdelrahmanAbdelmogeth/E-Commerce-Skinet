using ECommerceSkinet.Infrastructure.DatabaseContext;
using ECommerceSkinet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ECommerceSkinet.Core.Entities.OrderAggregate;
using ECommerceSkinet.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerceSkinet.Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, 
            ILoggerFactory loggerFactory,
            UserManager<AppUser> userManager)
        {
            try
            {
                if(!userManager.Users.Any(x => x.Email == "admin@test.com"))
                {
                    var user = new AppUser
                    {
                        UserName = "admin@test.com",
                        Email = "admin@test.com"
                    };
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Admin");
                }

                if (!context.ProductBrands.Any())
                {
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = @"..\..\..\..\SkiNetAPI.Infrastructure\Data\SeedData\brands.json";
                    var absolutePath = @"H:\WEB Development\E-Commerce Angular_.Net Core\SkiNet API\SkiNet API\SkiNet.Infrastructure\Data\SeedData\brands.json";
                    var fullPath = Path.Combine(basePath, relativePath);
                    var brandsData = File.ReadAllText(absolutePath);

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    context.Database.OpenConnection();
                    try
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductBrands ON");
                        foreach (var item in brands)
                            context.ProductBrands.Add(item);
                        await context.SaveChangesAsync();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductBrands OFF");
                    }
                    finally
                    {
                        context.Database.CloseConnection();
                    }

                }

                if (!context.ProductTypes.Any())
                {
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = @"..\..\..\..\SkiNetAPI.Infrastructure\Data\SeedData\types.json";
                    var fullPath = Path.Combine(basePath, relativePath);
                    var absolutePath = @"H:\WEB Development\E-Commerce Angular_.Net Core\SkiNet API\SkiNet API\SkiNet.Infrastructure\Data\SeedData\types.json";
                    var typesData = File.ReadAllText(absolutePath);

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    context.Database.OpenConnection();
                    try
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductTypes ON");
                        foreach (var item in types)
                            context.ProductTypes.Add(item);
                        await context.SaveChangesAsync();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductTypes OFF");
                    }
                    finally
                    {
                        context.Database.CloseConnection();
                    }
                }

                if (!context.Products.Any())
                {
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = @"..\..\..\..\SkiNet.Infrastructure\Data\SeedData\products.json";
                
                    var fullPath = Path.Combine(basePath, relativePath);
                    var absolutePath = @"H:\WEB Development\E-Commerce Angular_.Net Core\SkiNet API\SkiNet API\SkiNet.Infrastructure\Data\SeedData\products.json";
                    var productsData = File.ReadAllText(absolutePath);

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                        context.Products.Add(item);
                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = @"..\..\..\SkiNet.Infrastructure\Data\SeedData\delivery.json";

                    var fullPath = Path.Combine(basePath, relativePath);
                    var absolutePath = @"H:\WEB Development\E-Commerce Angular_.Net Core\SkiNet API\SkiNet API\SkiNet.Infrastructure\Data\SeedData\delivery.json";
                    var dmData = File.ReadAllText(absolutePath);

                    var dmMothods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    foreach (var item in dmMothods)
                        context.DeliveryMethods.Add(item);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
