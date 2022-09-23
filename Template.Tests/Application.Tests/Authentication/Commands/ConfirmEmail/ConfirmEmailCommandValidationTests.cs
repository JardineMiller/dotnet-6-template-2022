using FluentValidation.TestHelper;
using Template.Application.Authentication.Commands.ConfirmEmail;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidationTests
{
    private readonly ConfirmEmailValidation _validator;
    private const string validToken = "token";
    private const string validEmail = "test@user.com";

    public ConfirmEmailCommandValidationTests()
    {
        this._validator = new ConfirmEmailValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData("thing@thing")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
    )]
    public void Should_Have_Error_When_Email_Is_Invalid(
        string invalidEmail
    )
    {
        var command = new ConfirmEmailCommand(
            invalidEmail,
            validToken
        );

        var result = this._validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Token);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    public void Should_Have_Error_When_Token_Is_Invalid(
        string invalidToken
    )
    {
        var command = new ConfirmEmailCommand(
            validEmail,
            invalidToken
        );

        var result = this._validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_With_Valid_Input()
    {
        var command = new ConfirmEmailCommand(validEmail, validToken);

        var result = this._validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
    }
}
