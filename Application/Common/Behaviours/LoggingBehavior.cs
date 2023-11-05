using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Staring request {@RequestName}, {@DateTime}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogError("Request failure {@RequestName}, {@Error}, {@DateTime}",
                typeof(TRequest).Name,
                result.Error,
                DateTime.UtcNow);
        }

        _logger.LogInformation("Completed request {@RequestName}, {@DateTime}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}