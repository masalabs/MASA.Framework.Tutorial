using System.Reflection;
using FluentValidation;
using Masa.BuildingBlocks.Caching;
using Masa.BuildingBlocks.Data.UoW;
using Masa.BuildingBlocks.Ddd.Domain;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using Masa.BuildingBlocks.Dispatcher.Events;
using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;
using Masa.EShop.Service.Catalog.Infrastructure;
using Masa.EShop.Service.Catalog.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Register Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Services
    .AddMultilevelCache(cacheBuilder => cacheBuilder.UseStackExchangeRedisCache())
    .AddEventBus(eventBusBuilder => eventBusBuilder.UseMiddleware(typeof(ValidatorEventMiddleware<>)))
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddMasaDbContext<CatalogDbContext>(contextBuilder =>
    {
        contextBuilder
            .UseSqlite()
            .UseFilter();
    })
    //Use the same database as the write library (pseudo read-write separation)
    .AddMasaDbContext<CatalogQueryDbContext>(contextBuilder =>
    {
        contextBuilder
            .UseSqlite()
            .UseFilter();
    })
    .Configure<AuditEntityOptions>(options => options.UserIdType = typeof(int))
    .AddDomainEventBus(options =>
    {
        options
            .UseIntegrationEventBus(eventOptions=>eventOptions.UseDapr().UseEventLog<CatalogDbContext>())
            .UseEventBus()
            .UseUoW<CatalogDbContext>()
            .UseRepository<CatalogDbContext>();
    })
    .AddSequentialGuidGenerator();

var app = builder.AddServices();

app.UseMasaExceptionHandler(options =>
{
    options.ExceptionHandler = exceptionContext =>
    {
        if (exceptionContext.Exception is ArgumentNullException ex)
            exceptionContext.ToResult(ex.Message, 298);
    };
});

#region Use Swaager

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion

await app.MigrateDbContextAsync<CatalogDbContext>(async (context, services) =>
{
    await CatalogContextSeed.SeedAsync(context);
});

app.MapGet("/", () => "Hello World!");

app.Run();