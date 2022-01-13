using Discount.Grpc.Extensions;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.

services.AddScoped<IDiscountRepository, DiscountRepository>();
services.AddAutoMapper(typeof(Program));
services.AddGrpc();

var app = builder.Build();

app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
