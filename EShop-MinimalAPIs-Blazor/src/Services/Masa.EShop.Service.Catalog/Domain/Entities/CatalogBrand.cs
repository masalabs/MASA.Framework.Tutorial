using Masa.BuildingBlocks.Data;
using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;

namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogBrand : FullAggregateRoot<Guid, int>
{
    public string Brand { get; private set; } = "";

    private CatalogBrand()
    {
    }

    public CatalogBrand(Guid? id, string brand) : this()
    {
        Id = id ?? IdGeneratorFactory.SequentialGuidGenerator.NewId();
        Brand = brand;
    }
}