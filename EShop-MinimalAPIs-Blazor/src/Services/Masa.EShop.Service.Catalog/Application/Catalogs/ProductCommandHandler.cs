using Masa.BuildingBlocks.Caching;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using Masa.Contrib.Dispatcher.Events;
using Masa.EShop.Service.Catalog.Application.Catalogs.Commands;
using Masa.EShop.Service.Catalog.Domain.Entities;

namespace Masa.EShop.Service.Catalog.Application.Catalogs;

public class ProductCommandHandler
{
    private readonly IRepository<CatalogItem, Guid> _repository;
    private readonly IMultilevelCacheClient _multilevelCacheClient;

    public ProductCommandHandler(
        IRepository<CatalogItem, Guid> repository,
        IMultilevelCacheClient multilevelCacheClient)
    {
        _repository = repository;
        _multilevelCacheClient = multilevelCacheClient;
    }

    [EventHandler]
    public async Task CreateHandleAsync(CreateProductCommand command)
    {
        var catalogItem = new CatalogItem(command.Name, command.Price, command.PictureFileName ?? "default.png");
        catalogItem.SetCatalogType(command.CatalogTypeId);
        catalogItem.SetCatalogBrand(command.CatalogBrandId);
        catalogItem.AddStock(command.Stock);
        await _repository.AddAsync(catalogItem);
        await _multilevelCacheClient.SetAsync(catalogItem.Id.ToString(), catalogItem);
    }

    [EventHandler]
    public async Task DeleteHandlerAsync(DeleteProductCommand command)
    {
        await _repository.RemoveAsync(command.ProductId);
        await _multilevelCacheClient.RemoveAsync<CatalogItem>(command.ProductId.ToString());
    }
}