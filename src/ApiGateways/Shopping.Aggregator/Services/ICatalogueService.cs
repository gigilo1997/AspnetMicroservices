using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICatalogueService
{
    Task<IEnumerable<CatalogueModel>> GetCatalogue();
    Task<IEnumerable<CatalogueModel>> GetCatalogueByCategory(string category);
    Task<CatalogueModel> GetCatalogueById(string id);
}