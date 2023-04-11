using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
using Masa.EShop.Contracts.Catalog.Dto;
using Masa.Utils.Models;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Queries;

public record ProductsQuery : Query<PaginatedListBase<CatalogListItemDto>>
{
    public int PageSize { get; set; } = default!;

    public int Page { get; set; } = default!;

    public string? Name { get; set; }

    public bool IsRecycle { get; set; } = false;

    public override PaginatedListBase<CatalogListItemDto> Result { get; set; } = default!;
}