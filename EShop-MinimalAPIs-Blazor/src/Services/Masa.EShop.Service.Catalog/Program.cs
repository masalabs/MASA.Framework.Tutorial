using System.Reflection;
using FluentValidation;
using Masa.BuildingBlocks.Caching;
using Masa.BuildingBlocks.Dispatcher.Events;
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
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddMasaDbContext<CatalogDbContext>(contextBuilder =>
{
    contextBuilder
        .UseSqlite()
        .UseFilter();
});

//Use the same database as the write library (pseudo read-write separation)
builder.Services.AddMasaDbContext<CatalogQueryDbContext>(contextBuilder =>
{
    contextBuilder
        .UseSqlite()
        .UseFilter();
});

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
    var env = services.GetRequiredService<IWebHostEnvironment>();

    await CatalogContextSeed.SeedAsync(context, env);
});

app.MapGet("/", () => "Hello World!");

app.Run();