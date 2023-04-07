using Masa.BuildingBlocks.Data;

namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogBrand : ISoftDelete
{
    public int Id { get; set; }

    public string Brand { get; set; }
    
    public bool IsDeleted { get; private set; }
}