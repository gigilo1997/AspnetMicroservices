using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumers;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var services = builder.Services;

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});
services.AddMassTransitHostedService();

services.AddAutoMapper(typeof(Program));
services.AddScoped<BasketCheckoutConsumer>();

var app = builder.Build();

app.MigrateDatabase<OrderDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeeder>>();
    OrderContextSeeder.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
