using BasketAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BasketAPI.Rrpo
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly IDistributedCache _redis;

        public ShoppingCartRepo(IDistributedCache redis)
        {
            _redis = redis;
        }
        public async Task DeleteShoppingCart(string username)
        {
            await _redis.RemoveAsync(username);
        }

        public async Task<ShoppingCart> GetShoppingCart(string username)
        {
            var basket=await _redis.GetStringAsync(username);
            if(string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateShoppingCart(ShoppingCart shoppingCart)
        {
           await _redis.SetStringAsync(shoppingCart.UserName,JsonConvert.SerializeObject(shoppingCart));
            return await GetShoppingCart(shoppingCart.UserName);
        }
    }
}
