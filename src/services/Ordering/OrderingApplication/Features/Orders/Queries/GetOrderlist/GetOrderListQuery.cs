using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingApplication.Features.Orders.Queries.GetOrderlist
{
    public class GetOrderListQuery:IRequest<List<OrdersVm>>
    {
        public string UserName { get; set; }
        public GetOrderListQuery(string username)
        {
            UserName = username;
        }
    }
}
