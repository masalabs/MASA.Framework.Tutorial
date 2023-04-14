namespace Masa.EShop.Contracts.Catalog.Dto;

public class CatalogItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid CatalogBrandId { get; set; }

    public int CatalogTypeId { get; set; }

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = "default.png";
}