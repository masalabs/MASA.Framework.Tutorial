using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public record DeleteProductCommand : Command
{
    public Guid ProductId { get; set; }
}