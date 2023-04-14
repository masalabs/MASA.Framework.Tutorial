using Masa.BuildingBlocks.Ddd.Domain.SeedWork;
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

        await context.Database.EnsureCreatedAsync();
        if (!context.CatalogBrands.Any())
        {
            var catalogBrands = new List<CatalogBrand>()
            {
                new("LONSID")
            };
            await context.CatalogBrands.AddRangeAsync(catalogBrands);

            await context.SaveChangesAsync();
        }

        if (!context.CatalogTypes.Any())
        {
            var catalogTypes = GetCatalogTypes();
            await context.CatalogTypes.AddRangeAsync(catalogTypes);
            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<CatalogType> GetCatalogTypes()
    {
        return Enumeration.GetAll<CatalogType>();
    }
}