using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class CatalogueService : ICatalogueService
    {
        private readonly HttpClient _client;

        public CatalogueService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogueModel>> GetCatalogue()
        {
            var response = await _client.GetAsync("/Catalogue");
            return await response.ReadContentAs<List<CatalogueModel>>();
        }

        public async Task<CatalogueModel> GetCatalogue(string id)
        {
            var response = await _client.GetAsync($"/Catalogue/{id}");
            return await response.ReadContentAs<CatalogueModel>();
        }

        public async Task<IEnumerable<CatalogueModel>> GetCatalogueByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalogue/GetProductByCategory/{category}");
            return await response.ReadContentAs<List<CatalogueModel>>();
        }

        public async Task<CatalogueModel> CreateCatalogue(CatalogueModel model)
        {
            var response = await _client.PostAsJson($"/Catalogue", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CatalogueModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}