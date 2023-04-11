using Masa.BuildingBlocks.Caching;
using Masa.EShop.Service.Catalog.Infrastructure;
using Masa.Contrib.Dispatcher.Events;
using Masa.EShop.Service.Catalog.Application.Catalogs.Commands;
using Masa.EShop.Service.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Masa.EShop.Service.Catalog.Application.Catalogs;

public class ProductCommandHandler
{
    private readonly CatalogDbContext _dbContext;
    private readonly IMultilevelCacheClient _multilevelCacheClient;

    public ProductCommandHandler(CatalogDbContext dbContext, IMultilevelCacheClient multilevelCacheClient)
    {
        _dbContext = dbContext;
        _multilevelCacheClient = multilevelCacheClient;
    }

    [EventHandler]
    public async Task CreateHandleAsync(CreateProductCommand command)
    {
        var catalogItem = new CatalogItem()
        {
            CatalogBrandId = command.CatalogBrandId,
            CatalogTypeId = command.CatalogTypeId,
            Name = command.Name,
            PictureFileName = command.PictureFileName ?? "default.png",
            Price = command.Price
        };

        await _dbContext.CatalogItems.AddAsync(catalogItem);
        await _dbContext.SaveChangesAsync();
        
        await _multilevelCacheClient.SetAsync(catalogItem.Id.ToString(), catalogItem);
    }

    [EventHandler]
    public async Task DeleteHandlerAsync(DeleteProductCommand command)
    {
        var catalogItem = await _dbContext.CatalogItems.FirstOrDefaultAsync(item => item.Id == command.ProductId) ??
                          throw new UserFriendlyException("Product doesn't exist");
        _dbContext.CatalogItems.Remove(catalogItem);
        await _dbContext.SaveChangesAsync();
        
        await _multilevelCacheClient.RemoveAsync<CatalogItem>(catalogItem.Id.ToString());
    }
}