using BasketAPI.Models;

namespace BasketAPI.Rrpo
{
    public interface IShoppingCartRepo
    {
        Task<ShoppingCart> GetShoppingCart(string username);
        Task<ShoppingCart> UpdateShoppingCart(ShoppingCart shoppingCart);
        Task DeleteShoppingCart(string username);
    }
}
