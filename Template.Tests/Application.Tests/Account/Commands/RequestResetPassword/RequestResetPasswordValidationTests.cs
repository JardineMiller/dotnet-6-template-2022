using FluentValidation.TestHelper;
using Template.Application.Account.Commands.RequestResetPassword;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Account.Commands.RequestResetPassword;

public class ResetPasswordValidationTests
{
    private readonly RequestResetPasswordCommandValidation _validator;

    public ResetPasswordValidationTests()
    {
        this._validator = new RequestResetPasswordCommandValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("just_a_random_string")]
    private void Should_Have_Error_When_Email_Is_Invalid(
        string invalidEmail
    )
    {
        var query =
            new RequestResetPasswordCommand(
                invalidEmail
            );
        var result = this._validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("a@b.c")]
    [InlineData("email@test.com")]
    [InlineData("email@test")]
    private void Should_Not_Have_Error_When_Email_Is_Valid(
        string validEmail
    )
    {
        var query =
            new RequestResetPasswordCommand(
                validEmail
            );
        var result = this._validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
