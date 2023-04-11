using System.Linq.Expressions;
using Masa.BuildingBlocks.Caching;
using Masa.BuildingBlocks.Data;
using Masa.Contrib.Dispatcher.Events;
using Masa.EShop.Contracts.Catalog.Dto;
using Masa.EShop.Service.Catalog.Application.Catalogs.Queries;
using Masa.EShop.Service.Catalog.Domain.Entities;
using Masa.EShop.Service.Catalog.Infrastructure;
using Masa.Utils.Models;
using Microsoft.EntityFrameworkCore;

namespace Masa.EShop.Service.Catalog.Application.Catalogs;

public class ProductQueryHandler
{
    private readonly CatalogQueryDbContext _dbContext;

    public ProductQueryHandler(CatalogQueryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [EventHandler]
    public async Task ProductsHandleAsync(ProductsQuery query, IDataFilter dataFilter)
    {
        Expression<Func<CatalogItem, bool>> condition = item => true;
        condition = condition.And(!query.Name.IsNullOrWhiteSpace(), item => item.Name.Contains(query.Name!));

        if (!query.IsRecycle)
        {
            await GetItemsAsync();
        }
        else
        {
            using (dataFilter.Disable<ISoftDelete>())
            {
                condition = condition.And(item => item.IsDeleted); //Only query the data of the recycle bin
                await GetItemsAsync();
            }
        }

        async Task GetItemsAsync()
        {
            var queryable = _dbContext.CatalogItems.Where(condition);

            var total = await queryable.LongCountAsync();

            var totalPages = (int)Math.Ceiling((double)total / query.PageSize);

            var list = await queryable.Where(condition)
                .Select(item => new CatalogListItemDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    PictureFileName = item.PictureFileName,
                    CatalogTypeId = item.CatalogTypeId,
                    CatalogBrandId = item.CatalogBrandId,
                    Stock = item.Stock,
                })
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            query.Result = new PaginatedListBase<CatalogListItemDto>()
            {
                Total = total,
                TotalPages = totalPages,
                Result = list
            };
        }
    }

    [EventHandler]
    public async Task ProductHandleAsync(ProductQuery query, IMultilevelCacheClient multilevelCacheClient)
    {
        TimeSpan? memoryTimeSpan = null;
        var catalogItem = await multilevelCacheClient.GetOrSetAsync(query.ProductId.ToString(),
            async () =>
            {
                var item = await _dbContext.CatalogItems
                    .Where(item => item.Id == query.ProductId)
                    .Select(item => new CatalogItemDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        PictureFileName = item.PictureFileName,
                        CatalogTypeId = item.CatalogTypeId,
                        CatalogBrandId = item.CatalogBrandId
                    }).FirstOrDefaultAsync();
                
                memoryTimeSpan = item == null ? TimeSpan.FromSeconds(5) :TimeSpan.FromSeconds(60);
                
                return new CacheEntry<CatalogItemDto>(item);
            }, memoryOptions => memoryOptions.AbsoluteExpirationRelativeToNow = memoryTimeSpan);

        if (catalogItem == null)
            throw new UserFriendlyException("Product doesn't exist");
        query.Result = catalogItem;
    }
}