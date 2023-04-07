namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public CatalogType() { }

    public CatalogType(string type)
    {
        Type = type;
    }
}