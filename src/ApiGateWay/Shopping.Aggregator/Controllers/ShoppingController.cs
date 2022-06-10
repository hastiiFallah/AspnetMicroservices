using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ICatelogService _catelogService;
        private readonly IOrderService _orderService;

        public ShoppingController(IBasketService basketService, ICatelogService catelogService, IOrderService orderService)
        {
            _basketService = basketService;
            _catelogService = catelogService;
            _orderService = orderService;
        }
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);
            foreach (var item in basket.Items)
            {
                var product = await _catelogService.GetCatelog(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;

            }
            var order = await _orderService.GetOrderByuserName(userName);
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = order

            };
            return Ok(shoppingModel);
        }
    }
}
