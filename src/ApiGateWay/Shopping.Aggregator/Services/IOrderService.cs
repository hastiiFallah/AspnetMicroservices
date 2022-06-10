using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>>GetOrderByuserName(string userName);
    }
}
