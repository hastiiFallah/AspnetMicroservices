using AutoMapper;
using OrderingApplication.Features.Orders.Commands.CheckOutOrderCommand;
using OrderingApplication.Features.Orders.Commands.UpdateOrder;
using OrderingApplication.Features.Orders.Queries.GetOrderlist;
using OrderingDomain.Moddels;

namespace OrderingApplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
