using FluentValidation;
using MediatR;
using ValidationException = OrderingApplication.Exeptions.ValidationException;

namespace OrderingApplication.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var Results = await Task.WhenAll(_validators.Select(s => s.ValidateAsync(context, cancellationToken)));
                var fail = Results.SelectMany(s => s.Errors).Where(e => e != null).ToList();
                if (fail.Count != 0)
                    throw new ValidationException(fail);
            }
                return await next();

            }
        }
    }



