using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public record CreateProductCommand : Command
{
    public string Name { get; set; } = default!;

    /// <summary>
    /// seed data：31b1c60b-e9c3-4646-ac70-09354bdb1522
    /// </summary>
    public Guid CatalogBrandId { get; set; }

    /// <summary>
    /// seed data：1
    /// </summary>
    public int CatalogTypeId { get; set; } 

    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }

    public int Stock { get; set; }
}