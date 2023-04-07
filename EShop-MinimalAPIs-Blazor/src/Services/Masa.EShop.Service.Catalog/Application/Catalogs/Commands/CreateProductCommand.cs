using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public record CreateProductCommand : Command
{
    public string Name { get; set; } = default!;

    public int CatalogBrandId { get; set; }

    public int CatalogTypeId { get; set; }

    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }

    public int Stock { get; set; }
}