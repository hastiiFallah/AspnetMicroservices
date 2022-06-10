using AutoMapper;
using BasketAPI.Models;
using EventBus.Messages.Events;

namespace BasketAPI.Mapper
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, CheckOutBasketEvents>().ReverseMap();
        }
    }
}
