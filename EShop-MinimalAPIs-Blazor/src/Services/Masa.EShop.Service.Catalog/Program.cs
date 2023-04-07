using Masa.BuildingBlocks.Dispatcher.Events;
using Masa.EShop.Service.Catalog.Infrastructure;
using Masa.EShop.Service.Catalog.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region 注册Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Services.AddEventBus(eventBusBuilder => eventBusBuilder.UseMiddleware(typeof(ValidatorEventMiddleware<>)));

builder.Services.AddMasaDbContext<CatalogDbContext>(contextBuilder =>
{
    contextBuilder
        .UseSqlite()
        .UseFilter();
});

var app = builder.AddServices();

#region 使用Swaager

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