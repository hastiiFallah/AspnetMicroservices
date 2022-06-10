using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderingApplication.Behaviours
{
    public class UnhandledExeptionBehaviour <TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExeptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {

                var requestname = typeof(TRequest).Name;
                _logger.LogError(ex,"");
                throw;
            }
        }
    }
}
