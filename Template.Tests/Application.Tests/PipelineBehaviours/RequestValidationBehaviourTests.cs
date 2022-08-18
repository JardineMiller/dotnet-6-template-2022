using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation;
using Shouldly;
using Template.Application.Authentication.Common;
using Template.Application.Authentication.Queries.Login;
using Template.Application.PipelineBehaviours;
using Xunit;

namespace Template.Application.Tests.Application.Tests.PipelineBehaviours;

public class RequestValidationBehaviourTests
{
    private readonly List<IValidator<LoginQuery>> _validators =
        new() { new LoginQueryValidation() };

    [Fact]
    public async Task Should_NotContainValidationErrors_If_NoValidatorsProvided()
    {
        var requestValidationBehaviour =
            new RequestValidationBehaviour<
                LoginQuery,
                ErrorOr<AuthenticationResult>
            >(new List<IValidator<LoginQuery>>());

        var query = new LoginQuery("test@email.com", "password123!");

        var result = await requestValidationBehaviour.Handle(
            query,
            CancellationToken.None,
            () =>
            {
                return Task.Run(
                    () => new ErrorOr<AuthenticationResult>()
                );
            }
        );

        result.IsError.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_NotContainValidationErrors_If_ValidatorsProvided_ButInputIsValid()
    {
        var requestValidationBehaviour =
            new RequestValidationBehaviour<
                LoginQuery,
                ErrorOr<AuthenticationResult>
            >(this._validators);

        var query = new LoginQuery("test@email.com", "Password123!");

        var result = await requestValidationBehaviour.Handle(
            query,
            CancellationToken.None,
            () =>
            {
                return Task.Run(
                    () => new ErrorOr<AuthenticationResult>()
                );
            }
        );

        result.IsError.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_ContainValidationErrors_If_InputIsInvalid()
    {
        var requestValidationBehaviour =
            new RequestValidationBehaviour<
                LoginQuery,
                ErrorOr<AuthenticationResult>
            >(this._validators);

        var query = new LoginQuery("test@email.com", "");

        var result = await requestValidationBehaviour.Handle(
            query,
            CancellationToken.None,
            () =>
            {
                return Task.Run(
                    () => new ErrorOr<AuthenticationResult>()
                );
            }
        );

        result.IsError.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
    }
}
