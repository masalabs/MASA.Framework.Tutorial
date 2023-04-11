using Masa.EShop.Service.Catalog.Domain.Entities;
using Masa.EShop.Service.Catalog.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Masa.EShop.Service.Catalog.Infrastructure;

public class CatalogQueryDbContext : MasaDbContext<CatalogQueryDbContext>
{
    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    public CatalogQueryDbContext(MasaDbContextOptions<CatalogQueryDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreatingExecuting(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogBrandEntityTypeConfiguration).Assembly);
        base.OnModelCreatingExecuting(builder);
    }
}