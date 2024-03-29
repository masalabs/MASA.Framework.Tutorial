﻿using Masa.BuildingBlocks.Data.Contracts;
using Masa.EShop.Service.Catalog.Domain.Entities;

namespace Masa.EShop.Service.Catalog.Infrastructure.Extensions;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogDbContext context)
    {
        await context.Database.EnsureCreatedAsync();
        if (!context.CatalogBrands.Any())
        {
            var catalogBrands = new List<CatalogBrand>()
            {
                new(Guid.Parse("31b1c60b-e9c3-4646-ac70-09354bdb1522"), "LONSID")
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