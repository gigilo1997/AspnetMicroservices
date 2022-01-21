using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = configuration["CacheSettings:ConnectionString"];
});

services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
    o.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));

services.AddScoped<DiscountGrpcService>();

services.AddScoped<IBasketRepository, BasketRepository>();

services.AddAutoMapper(typeof(Program));

services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration["EventBusSettings:HostAddress"]);
    });
});
services.AddMassTransitHostedService();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
