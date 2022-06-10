using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Persistense;
using OrderingApplication.Exeptions;
using OrderingDomain.Moddels;

namespace OrderingApplication.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        private readonly IOrderRepo _orderRepo;

        public DeleteOrderCommandHandler(IMapper mapper, ILogger<DeleteOrderCommandHandler> logger, IOrderRepo orderRepo)
        {
            _mapper = mapper;
            _logger = logger;
            _orderRepo = orderRepo;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepo.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            await _orderRepo.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order with id:{orderToDelete.Id} Deleted Successfuly");
            return Unit.Value;
        }
    }
}
