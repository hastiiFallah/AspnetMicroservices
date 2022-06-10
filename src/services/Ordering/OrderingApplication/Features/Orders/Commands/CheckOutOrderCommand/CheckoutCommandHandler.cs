using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Infrastructure;
using OrderingApplication.Contracts.Persistense;
using OrderingDomain.Moddels;

namespace OrderingApplication.Features.Orders.Commands.CheckOutOrderCommand
{
    internal class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger<CheckoutCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public CheckoutCommandHandler(IMapper mapper, IOrderRepo orderRepo, ILogger<CheckoutCommandHandler> logger, IEmailService emailService)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
            _logger = logger;
            _emailService = emailService;
        }
        public async Task<int> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var neworder = await _orderRepo.AddAsync(orderEntity);
            _logger.LogInformation($"Order with id {neworder.Id} created succesfully");
            await SendMail(neworder);
            return neworder.Id;
        }
        public async Task SendMail(Order order)
        {
            var email = new Models.Email { To = "ezozkme@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"order with id {order.Id} failed due : {ex.Message}");

            }
        }
    }
}
