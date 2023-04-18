using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;

namespace Masa.EShop.Contracts.Catalog.IntegrationEvents;

public record CatalogCreatedIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? PictureFileName { get; set; }

    public virtual int CatalogTypeId { get; set; }

    public virtual Guid CatalogBrandId { get; set; }
}

