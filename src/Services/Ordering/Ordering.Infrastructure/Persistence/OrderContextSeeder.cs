using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeeder
{
    public static async Task SeedAsync(OrderDbContext dbContext, ILogger<OrderContextSeeder> logger)
    {
        if (!dbContext.Orders.Any())
        {
            dbContext.Orders.AddRange(GetPreconfiguredOrders());
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderDbContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
            {
                new Order() {Username = "swn", FirstName = "Mehmet", LastName = "Ozkaya", EmailAddress = "ezozkme@gmail.com", AddressLine = "Bahcelievler", Country = "Turkey", TotalPrice = 350 }
            };
    }
}
