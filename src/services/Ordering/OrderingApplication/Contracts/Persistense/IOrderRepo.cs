using OrderingDomain.Moddels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingApplication.Contracts.Persistense
{
    public interface IOrderRepo:IAsyncRepo<Order>
    {
        Task<IEnumerable<Order>> GetOrderByUsername(string username);
    }
}
