using AutoMapper;
using EventBus.Messages.Events;
using OrderingApplication.Features.Orders.Commands.CheckOutOrderCommand;

namespace OrderingAPI.Mapper
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<CheckoutCommand, CheckOutBasketEvents>().ReverseMap();
        }
    }
}
