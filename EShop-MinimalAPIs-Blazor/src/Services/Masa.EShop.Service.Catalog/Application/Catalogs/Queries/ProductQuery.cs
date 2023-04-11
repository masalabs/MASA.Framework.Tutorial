using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
using Masa.EShop.Contracts.Catalog.Dto;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Queries;

public record ProductQuery : Query<CatalogItemDto>
{
    public int ProductId { get; set; } = default!;
    public override CatalogItemDto Result { get; set; } = default!;
}