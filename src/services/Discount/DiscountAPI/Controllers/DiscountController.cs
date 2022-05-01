using DiscountAPI.Models;
using DiscountAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DiscountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepo _discount;

        public DiscountController(IDiscountRepo discount)
        {
            _discount = discount;
        }
        [HttpGet("{productname}")]
        public async Task<ActionResult<Cupon>> GetCupon(string productname)
        {
            var cupon = await _discount.GetCupon(productname);
            return Ok(cupon);
        }
        [HttpPost]
        public async Task<ActionResult<Cupon>> CreateCupon([FromBody] Cupon Cuopn)
        {
            await _discount.CreateCupon(Cuopn);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<Cupon>> UpdateCupon([FromBody] Cupon Cuopn)
        {
            return Ok(await _discount.UpdateCupon(Cuopn));

        }
        [HttpDelete("{CuopnName}")]
        public async Task<ActionResult<Cupon>> DeleteCupon(string CuopnName)
        {
            return Ok(await _discount.DeleteCupon(CuopnName));
        }
    }
}
