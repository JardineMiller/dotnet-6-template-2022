using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Template.Application.PipelineBehaviours;

public class RequestValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>>? _validators;

    public RequestValidationBehaviour(
        IEnumerable<IValidator<TRequest>> validators
    )
    {
        this._validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next
    )
    {
        if (this._validators is null || !this._validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var failures = this._validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            return TryCreateErrorResponseFromErrors(
                failures,
                out var errorResponse
            )
              ? errorResponse
              : throw new ValidationException(failures);
        }

        return await next();
    }

    private static bool TryCreateErrorResponseFromErrors(
        List<ValidationFailure> failures,
        out TResponse response
    )
    {
        var errors = failures.ConvertAll(
            x =>
                Error.Validation(
                    code: x.PropertyName,
                    description: x.ErrorMessage
                )
        );

        try
        {
            response = (TResponse)(dynamic)errors;
            return true;
        }
        catch (Exception)
        {
            response = default!;
            return false;
        }
    }
}