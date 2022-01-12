using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = configuration["CacheSettings:ConnectionString"];
});

services.AddScoped<IBasketRepository, BasketRepository>();

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
