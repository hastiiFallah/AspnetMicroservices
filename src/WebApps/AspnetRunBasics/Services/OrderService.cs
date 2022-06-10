using AspnetRunBasics.Models;
using Shopping.Aggregator.Extentions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<OrderResponseModel>> GetOrderByUsername(string userName)
        {
            var respone = await _httpClient.GetAsync($"/Order/{userName}");
            return await respone.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
