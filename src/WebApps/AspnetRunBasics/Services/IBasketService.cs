using AspnetRunBasics.Models;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public interface IBasketService
    {
        Task<BasketModel>GetBaket(string id);
        Task<BasketModel>UpdateBasket(BasketModel basketModel);
        Task CheckoutBasket(CheckoutModel model);
    }
}
