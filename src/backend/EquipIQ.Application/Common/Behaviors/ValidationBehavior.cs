using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace EquipIQ.Application.Common.Behaviors;

/// <summary>
/// Runs every FluentValidation validator for a command or query BEFORE its handler.
/// If anything is invalid, it throws error and the handler never runs. 
/// A message with no validator just passes through.
/// </summary>
/// /// 
/// <remarks>
/// The where TMessage : IMessage and <TMessage, TResponse> parts just mean “this works for any command or query, whatever its type.” 
/// That’s why a single class covers everything.
public sealed class ValidationBehavior<TMessage, TResponse>(IEnumerable<IValidator<TMessage>> validators)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();
        var context = new ValidationContext<TMessage>(message);

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
        {
            // The API's error middleware (Step 14) turns this into a 400 response.
            throw new ValidationException(failures);
        }

        // No validation errors, so pass to the next station (the actual handler)
        return await next(message, cancellationToken); ///////
    }
}