using Masa.BuildingBlocks.Data;
using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;
using Masa.EShop.Service.Catalog.Domain.Events;

namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogItem : FullAggregateRoot<Guid, int>
{
    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public string PictureFileName { get; private set; } = "";

    public int CatalogTypeId { get; private set; }

    public CatalogType CatalogType { get; private set; } = null!;

    public Guid CatalogBrandId { get; private set; }

    public CatalogBrand CatalogBrand { get; private set; } = null!;

    public int Stock { get; private set; }

    public CatalogItem(string name, decimal price, string pictureFileName)
    {
        Id = IdGeneratorFactory.SequentialGuidGenerator.NewId();
        Name = name;
        Price = price;
        PictureFileName = pictureFileName;
        AddCatalogDomainIntegrationEvent();
    }

    private void AddCatalogDomainIntegrationEvent()
    {
        var catalogCreatedIntegrationDomainEvent = new CatalogCreatedIntegrationDomainEvent(this);
        this.AddDomainEvent(catalogCreatedIntegrationDomainEvent);
    }

    public void SetCatalogType(int catalogTypeId)
    {
        CatalogTypeId = catalogTypeId;
    }

    public void SetCatalogBrand(Guid catalogBrand)
    {
        CatalogBrandId = catalogBrand;
    }

    public void AddStock(int stock)
    {
        Stock += stock;
    }
}