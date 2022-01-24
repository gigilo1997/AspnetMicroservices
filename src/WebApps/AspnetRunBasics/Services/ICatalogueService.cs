using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;

public interface ICatalogueService
{

    Task<IEnumerable<CatalogueModel>> GetCatalogue();
    Task<IEnumerable<CatalogueModel>> GetCatalogueByCategory(string category);
    Task<CatalogueModel> GetCatalogue(string id);
    Task<CatalogueModel> CreateCatalogue(CatalogueModel model);
}
