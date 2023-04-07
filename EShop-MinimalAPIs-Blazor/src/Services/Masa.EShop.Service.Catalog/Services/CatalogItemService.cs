using System.Linq.Expressions;
using Masa.BuildingBlocks.Data;
using Masa.EShop.Contracts.Catalog.Dto;
using Masa.EShop.Service.Catalog.Application.Catalogs.Commands;
using Masa.EShop.Service.Catalog.Domain.Entities;
using Masa.EShop.Service.Catalog.Infrastructure;
using Masa.Utils.Models;
using Microsoft.EntityFrameworkCore;

namespace Masa.EShop.Service.Catalog.Services;

public class CatalogItemService : ServiceBase
{
    private CatalogDbContext DbContext => GetRequiredService<CatalogDbContext>();

    public async Task<IResult> GetAsync(int id)
    {
        if (id <= 0)
            throw new UserFriendlyException("商品id必须大于0");

        var productInfo = await DbContext.CatalogItems.Where(item => item.Id == id).Select(item => new CatalogItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            PictureFileName = item.PictureFileName,
            CatalogTypeId = item.CatalogTypeId,
            CatalogBrandId = item.CatalogBrandId
        }).FirstOrDefaultAsync();
        if (productInfo == null)
            throw new UserFriendlyException("不存在的产品");

        return Results.Ok(productInfo);
    }

    /// <summary>
    /// `PaginatedListBase`由**Masa.Utils.Models.Config**提供, 如需使用，请安装`Masa.Utils.Models.Config`
    /// </summary>
    /// <returns></returns>
    public async Task<IResult> GetItemsAsync(
        string? name,
        int page = 0,
        int pageSize = 10)
    {
        if (page <= 0)
            throw new UserFriendlyException("页码必须大于0");

        if (pageSize <= 0)
            throw new UserFriendlyException("页大小必须大于0");

        Expression<Func<CatalogItem, bool>> condition = item => true;
        condition = condition.And(!name.IsNullOrWhiteSpace(), item => item.Name.Contains(name));
        var queryable = DbContext.CatalogItems.Where(condition);
        var total = await queryable.LongCountAsync();
        var list = await queryable.Where(condition).Select(item => new CatalogListItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            PictureFileName = item.PictureFileName,
            CatalogTypeId = item.CatalogTypeId,
            CatalogBrandId = item.CatalogBrandId,
            Stock = item.Stock,
        }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var pageData = new PaginatedListBase<CatalogListItemDto>()
        {
            Total = total,
            TotalPages = (int)Math.Ceiling((double)total / pageSize),
            Result = list
        };
        return Results.Ok(pageData);
    }
    
    /// <summary>
    /// Show only deleted listings
    /// </summary>
    public async Task<IResult> GetRecycleItemsAsync(
        string? name,
        IDataFilter dataFilter,
        int page = 0,
        int pageSize = 10)
    {
        if (page <= 0)
            throw new UserFriendlyException("页码必须大于0");

        if (pageSize <= 0)
            throw new UserFriendlyException("页大小必须大于0");

        Expression<Func<CatalogItem, bool>> condition = item => item.IsDeleted;
        condition = condition.And(!name.IsNullOrWhiteSpace(), item => item.Name.Contains(name));

        using (dataFilter.Disable<ISoftDelete>())
        {
            var queryable = DbContext.CatalogItems.Where(condition);
            var total = await queryable.LongCountAsync();
            var list = await queryable.Where(condition).Select(item => new CatalogListItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                PictureFileName = item.PictureFileName,
                CatalogTypeId = item.CatalogTypeId,
                CatalogBrandId = item.CatalogBrandId,
                Stock = item.Stock,
            }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pageData = new PaginatedListBase<CatalogListItemDto>()
            {
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                Result = list
            };
            return Results.Ok(pageData);
        }
    }

    public async Task<IResult> CreateProductAsync(CreateProductCommand command)
    {
        if (command.Name.IsNullOrWhiteSpace())
            throw new UserFriendlyException("产品名不能为空");

        var catalogItem = new CatalogItem()
        {
            CatalogBrandId = command.CatalogBrandId,
            CatalogTypeId = command.CatalogTypeId,
            Name = command.Name,
            PictureFileName = command.PictureFileName ?? "default.png",
            Price = command.Price
        };

        await DbContext.CatalogItems.AddAsync(catalogItem);
        await DbContext.SaveChangesAsync();
        return Results.Accepted();
    }

    public async Task<IResult> DeleteProductAsync(int id)
    {
        if (id <= 0)
            throw new UserFriendlyException("商品id必须大于0");

        var productInfo = await DbContext.CatalogItems.FirstOrDefaultAsync(item => item.Id == id);
        if (productInfo == null)
            throw new UserFriendlyException("不存在的产品");

        DbContext.CatalogItems.Remove(productInfo);
        await DbContext.SaveChangesAsync();

        return Results.Accepted();
    }
}