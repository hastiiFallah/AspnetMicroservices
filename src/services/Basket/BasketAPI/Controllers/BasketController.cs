
using AutoMapper;
using BasketAPI.gRPC_Services;
using BasketAPI.Models;
using BasketAPI.Rrpo;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IShoppingCartRepo _cartRepo;
        private readonly DiscountGrpcServices _grpcServices;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IShoppingCartRepo cartRepo,DiscountGrpcServices grpcServices,IPublishEndpoint publishEndpoint,IMapper mapper)
        {
            _cartRepo = cartRepo;
            _grpcServices = grpcServices;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _cartRepo.GetShoppingCart(username);
            return Ok(basket ?? new ShoppingCart(username));
        }
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.items)
            {
                var cupon = await _grpcServices.GetDiscountAsync(item.ProductName);
                item.Price -= cupon.Amount;

            }
            return Ok(await _cartRepo.UpdateShoppingCart(basket));
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _cartRepo.DeleteShoppingCart(username);
            return Ok();

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult>Checkout([FromBody]BasketCheckout basket)
        {
            var repo = await _cartRepo.GetShoppingCart(basket.UserName);
            if (repo == null)
            {
                return NotFound();
            }
            var evenmessage = _mapper.Map<CheckOutBasketEvents>(basket);
            evenmessage.TotalPrice=basket.TotalPrice;
            await _publishEndpoint.Publish(evenmessage);
            await _cartRepo.DeleteShoppingCart(basket.UserName);
            return Accepted();
        }
    }
}
