using Microsoft.EntityFrameworkCore;

namespace Masa.EShop.Service.Catalog.Infrastructure.Extensions;

public static class HostExtensions
{
    public static Task MigrateDbContextAsync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TContext>();
        return seeder(context, services);
    }
}