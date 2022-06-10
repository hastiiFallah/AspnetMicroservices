using AutoMapper;
using MediatR;
using OrderingApplication.Contracts.Persistense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingApplication.Features.Orders.Queries.GetOrderlist
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public GetOrderListQueryHandler(IOrderRepo orderRepo,IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
           var orderlist= await _orderRepo.GetOrderByUsername(request.UserName);
            return _mapper.Map<List<OrdersVm>>(orderlist);
        }
    }
}
