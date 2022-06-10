using AspnetRunBasics.Models;
using Shopping.Aggregator.Extentions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CheckoutBasket(CheckoutModel model)
        {
            var response=await _httpClient.PostAsJson($"/Basket/Checkout",model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<BasketModel> GetBaket(string id)
        {
            var response = await _httpClient.GetAsync($"/Basket/{id}");
            return await response.ReadContentAs<BasketModel>();

        }

        public async Task<BasketModel> UpdateBasket(BasketModel basketModel)
        {
            var response = await _httpClient.PostAsJson($"/Basket", basketModel);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<BasketModel>();
            else
                throw new Exception("Something went wrong when calling api.");
        }
    }
}
