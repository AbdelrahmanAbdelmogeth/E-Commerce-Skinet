using AccessOperationTeam.Infrastructure.DatabaseContext;
using ECommerceSkinet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerceSkinet.Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if(!context.ProductBrands.Any())
                {
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = @"..\..\..\..\AccessOperationTeam.Infrastructure\Data\SeedData\brands.json";
                    var fullPath = Path.Combine(basePath, relativePath);
                    var brandsData = File.ReadAllText(fullPath);

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
                    var relativePath = @"..\..\..\..\AccessOperationTeam.Infrastructure\Data\SeedData\types.json";
                    var fullPath = Path.Combine(basePath, relativePath);
                    var typesData = File.ReadAllText(fullPath);

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
                    var relativePath = @"..\..\..\..\AccessOperationTeam.Infrastructure\Data\SeedData\products.json";
                    var fullPath = Path.Combine(basePath, relativePath);
                    var productsData = File.ReadAllText(fullPath);

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                        context.Products.Add(item);
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
