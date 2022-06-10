using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderingApplication.Features.Orders.Commands.CheckOutOrderCommand;
using OrderingApplication.Features.Orders.Commands.DeleteOrder;
using OrderingApplication.Features.Orders.Commands.UpdateOrder;
using OrderingApplication.Features.Orders.Queries.GetOrderlist;

namespace OrderingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{userName}")]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrderByUsername(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        [HttpPost]
        public async Task<ActionResult<int>> CheckoutOrder(CheckoutCommand checkout)
        {
            var result = await _mediator.Send(checkout);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrder(UpdateOrderCommand updateOrder)
        {
            await _mediator.Send(updateOrder);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
