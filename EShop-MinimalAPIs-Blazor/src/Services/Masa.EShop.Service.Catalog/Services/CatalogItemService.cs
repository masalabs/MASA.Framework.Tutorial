using Masa.BuildingBlocks.Data;
using Masa.BuildingBlocks.Dispatcher.Events;
using Masa.EShop.Service.Catalog.Application.Catalogs.Commands;
using Masa.EShop.Service.Catalog.Application.Catalogs.Queries;

namespace Masa.EShop.Service.Catalog.Services;

public class CatalogItemService : ServiceBase
{
    private IEventBus EventBus => GetRequiredService<IEventBus>();

    public async Task<IResult> GetAsync(Guid id)
    {
        var query = new ProductQuery() { ProductId = id };
        await EventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    /// <summary>
    /// `PaginatedListBase` is provided by **Masa.Utils.Models.Config**, if you want to use it, please install `Masa.Utils.Models.Config`
    /// </summary>
    /// <returns></returns>
    public async Task<IResult> GetItemsAsync(
        string? name,
        int page = 1,
        int pageSize = 10)
    {
        var query = new ProductsQuery()
        {
            Name = name,
            Page = page,
            PageSize = pageSize
        };
        await EventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    /// <summary>
    /// Show only deleted listings
    /// </summary>
    public async Task<IResult> GetRecycleItemsAsync(
        string? name,
        int page = 1,
        int pageSize = 10)
    {
        var query = new ProductsQuery()
        {
            Name = name,
            IsRecycle = true,
            Page = page,
            PageSize = pageSize
        };
        await EventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    public async Task<IResult> CreateProductAsync(CreateProductCommand command)
    {
        await EventBus.PublishAsync(command);
        return Results.Accepted();
    }

    public async Task<IResult> DeleteProductAsync(Guid id)
    {
        await EventBus.PublishAsync(new DeleteProductCommand() { ProductId = id });

        return Results.Accepted();
    }
}