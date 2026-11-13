using System.Diagnostics;
using EquipIQ.Core.Common;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;

namespace EquipIQ.Application.Common.Behaviors;

/// <summary>
/// Used in Commands 
/// Writes a log line when a command or query starts, finishes, or fails, with timing.
/// Invalid input gets a warning (the user’s mistake), and a real crash gets an error (our bug).
/// </summary>
/// 
/// <remarks>
/// The where TMessage : IMessage and <TMessage, TResponse> parts just mean “this works for any command or query, whatever its type.” 
/// That’s why a single class covers everything.
public sealed class LoggingBehavior<TMessage, TResponse>(
    ILogger<LoggingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var messageName = typeof(TMessage).Name;
        var startedAt = Stopwatch.GetTimestamp();

        logger.LogInformation("Handling {MessageName}", messageName);

        try
        {
            // pass to next station (ValidationBehavior, then the actual handler)
            var response = await next(message, cancellationToken); //////

            logger.LogInformation(
                "Handled {MessageName} in {ElapsedMilliseconds} ms",
                messageName,
                Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);

            return response;
        }
        catch (Exception exception) when (exception is DomainException or ValidationException)
        {
            // Expected problems: the caller sent something invalid. A warning, not an error.
            logger.LogWarning(
                "{MessageName} was rejected: {Reason}",
                messageName,
                exception.Message);
            throw;
        }
        catch (Exception exception)
        {
            // Unexpected problems: a real bug or an outage. Log the full details.
            logger.LogError(exception, "{MessageName} failed unexpectedly", messageName);
            throw;
        }
    }
}