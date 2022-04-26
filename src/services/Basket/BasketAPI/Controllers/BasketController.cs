using BasketAPI.Models;
using BasketAPI.Rrpo;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IShoppingCartRepo _cartRepo;

        public BasketController(IShoppingCartRepo cartRepo)
        {
            _cartRepo = cartRepo;
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
            return Ok(await _cartRepo.UpdateShoppingCart(basket));
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _cartRepo.DeleteShoppingCart(username);
            return Ok();

        }
    }
}
