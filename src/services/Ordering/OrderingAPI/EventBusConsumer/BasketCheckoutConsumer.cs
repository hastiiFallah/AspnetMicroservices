using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using OrderingApplication.Features.Orders.Commands.CheckOutOrderCommand;

namespace OrderingAPI.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<CheckOutBasketEvents>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMapper mapper,IMediator mediator,ILogger<BasketCheckoutConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<CheckOutBasketEvents> context)
        {
            var command = _mapper.Map<CheckoutCommand>(context.Message);
            var result = await _mediator.Send(command);

            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);

        }
    }
}
