using Masa.BuildingBlocks.Data.Contracts;

namespace Masa.EShop.Service.Catalog.Domain.Entities;

public class CatalogType : Enumeration
{
    public static CatalogType WaterDispenser = new(1, "Water Dispenser");

    public CatalogType(int id, string name) : base(id, name)
    {
    }
}