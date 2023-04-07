using Masa.EShop.Service.Catalog.Domain.Entities;

namespace Masa.EShop.Service.Catalog.Infrastructure.Extensions;

public class CatalogContextSeed
{
    public static async Task SeedAsync(
        CatalogDbContext context,
        IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return;
        
        if (!context.CatalogBrands.Any())
        {
            var catalogBrands = new List<CatalogBrand>()
            {
                new()
                {
                    Id = 1,
                    Brand = "LONSID"
                }
            };
            await context.CatalogBrands.AddRangeAsync(catalogBrands);

            await context.SaveChangesAsync();
        }

        if (!context.CatalogTypes.Any())
        {
            var catalogTypes = new List<CatalogType>()
            {
                new()
                {
                    Id = 1,
                    Type = "Water Dispenser"
                }
            };
            await context.CatalogTypes.AddRangeAsync(catalogTypes);
            await context.SaveChangesAsync();
        }
    }
}