using Masa.BuildingBlocks.Ddd.Domain.Events;
using Masa.EShop.Contracts.Catalog.IntegrationEvents;
using Masa.EShop.Service.Catalog.Domain.Entities;

namespace Masa.EShop.Service.Catalog.Domain.Events;

public record CatalogCreatedIntegrationDomainEvent : CatalogCreatedIntegrationEvent, IIntegrationDomainEvent
{
    private readonly CatalogItem _catalog;

    private int? _catalogTypeId;

    public override int CatalogTypeId
    {
        get => _catalogTypeId ??= _catalog.CatalogTypeId;
        set => _catalogTypeId = value;
    }

    private Guid? _catalogBrandId;

    public override Guid CatalogBrandId
    {
        get => _catalogBrandId ??= _catalog.CatalogBrandId;
        set => _catalogBrandId = value;
    }

    public CatalogCreatedIntegrationDomainEvent(CatalogItem catalog) : base()
    {
        _catalog = catalog;
    }
}