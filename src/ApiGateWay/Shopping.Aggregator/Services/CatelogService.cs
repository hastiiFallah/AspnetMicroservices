using Shopping.Aggregator.Extentions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatelogService : ICatelogService
    {
        private readonly HttpClient _httpClient;

        public CatelogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CatelogModel>> GetCatelog()
        {
            var response = await _httpClient.GetAsync("/api/Catelog");
            return await response.ReadContentAs<List<CatelogModel>>();
        }

        public async Task<CatelogModel> GetCatelog(string id)
        {
            var response = await _httpClient.GetAsync($"/api/Catelog/{id}");
            return await response.ReadContentAs<CatelogModel>();
        }

        public async Task<IEnumerable<CatelogModel>> GetCatelogByCategory(string categoryName)
        {
            var response = await _httpClient.GetAsync($"/api/Catelog/GetProductByCategory/{categoryName}");
            return await response.ReadContentAs<List<CatelogModel>>();
        }
    }
}
