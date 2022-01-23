using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogueService : ICatalogueService
{
    private readonly HttpClient _client;

    public CatalogueService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<CatalogueModel>> GetCatalogue()
    {
        var response = await _client.GetAsync("/api/v1/Catalogue");
        return await response.ReadContentAs<List<CatalogueModel>>();
    }

    public async Task<CatalogueModel> GetCatalogueById(string id)
    {
        var response = await _client.GetAsync($"/api/v1/Catalogue/{id}");
        return await response.ReadContentAs<CatalogueModel>();
    }

    public async Task<IEnumerable<CatalogueModel>> GetCatalogueByCategory(string category)
    {
        var response = await _client.GetAsync($"/api/v1/Catalogue/GetProductByCategory/{category}");
        return await response.ReadContentAs<List<CatalogueModel>>();
    }
}