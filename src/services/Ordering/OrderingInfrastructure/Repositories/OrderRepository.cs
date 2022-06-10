using Microsoft.EntityFrameworkCore;
using OrderingApplication.Contracts.Persistense;
using OrderingDomain.Moddels;
using OrderingInfrastructure.Persistence;

namespace OrderingInfrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepo
    {
        public OrderRepository(OrderContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Order>> GetOrderByUsername(string username)
        {
            var orderlist = await _dbcontext.orders.Where(o => o.UserName == username)
                .ToListAsync();
            return orderlist;
        }
    }
}
