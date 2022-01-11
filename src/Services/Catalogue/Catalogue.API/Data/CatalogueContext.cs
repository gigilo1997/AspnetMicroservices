using Catalogue.API.Entities;
using MongoDB.Driver;

namespace Catalogue.API.Data;

public class CatalogueContext : ICatalogueContext
{
    public CatalogueContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

        Products = database.GetCollection<Product>(configuration["DatabaseSettings:CollectionName"]);
        CatalogueContextSeeder.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; set; }
}
