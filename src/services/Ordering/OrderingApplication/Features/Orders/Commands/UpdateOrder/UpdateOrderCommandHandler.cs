using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Persistense;
using OrderingApplication.Exeptions;
using OrderingDomain.Moddels;

namespace OrderingApplication.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepo _order;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IMapper mapper,IOrderRepo order,ILogger<UpdateOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _order = order;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var ordertoUpdate=await _order.GetByIdAsync(request.Id);
            if (ordertoUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request,ordertoUpdate,typeof(UpdateOrderCommand),typeof(Order));
            await _order.UpdateAsync(ordertoUpdate);
            _logger.LogInformation($"order with id:{ordertoUpdate.Id} Updated Succesfully");
            return Unit.Value;
        }
    }
}
