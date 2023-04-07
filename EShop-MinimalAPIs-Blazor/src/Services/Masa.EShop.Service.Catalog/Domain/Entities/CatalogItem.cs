using Masa.BuildingBlocks.Data;

namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogItem : ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = "";

    public int CatalogTypeId { get; set; }

    public CatalogType CatalogType { get; private set; } = null!;

    public int CatalogBrandId { get; set; }

    public CatalogBrand CatalogBrand { get; private set; } = null!;

    public int Stock { get; set; }

    public bool IsDeleted { get; private set; }
}