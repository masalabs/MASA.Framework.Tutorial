using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using Masa.EShop.Service.Catalog.Domain.Entities;

namespace Masa.EShop.Service.Catalog.Domain.Repositories;

public interface ICatalogItemRepository : IRepository<CatalogItem, Guid>
{
    
}