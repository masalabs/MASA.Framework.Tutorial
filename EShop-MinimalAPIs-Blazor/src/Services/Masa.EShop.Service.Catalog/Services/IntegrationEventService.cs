using Dapr;
using Masa.EShop.Contracts.Catalog.IntegrationEvents;

namespace Masa.EShop.Service.Catalog.Services;

public class IntegrationEventService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";

    private ILogger<IntegrationEventService> _logger => GetRequiredService<ILogger<IntegrationEventService>>();

    [Topic(DAPR_PUBSUB_NAME, nameof(CatalogCreatedIntegrationEvent))]
    public Task NoticeWarehouseAdministratorByCatalogCreated(CatalogCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("New product: {Name}, Id: {Id}", @event.Name, @event.Id);
        //todo: Notify warehouse clerks of new products
        return Task.CompletedTask;
    }
}