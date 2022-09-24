using FluentValidation.TestHelper;
using Template.Application.Account.Commands;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Account.Commands;

public class ResetPasswordValidationTests
{
    private readonly ResetPasswordCommandValidation _validator;

    private const string validPassword = "Password123!";
    private const string validEmail = "test@user.com";
    private const string validToken = "token";

    public ResetPasswordValidationTests()
    {
        this._validator = new ResetPasswordCommandValidation();
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
        var query = new ResetPasswordCommand(
            invalidEmail,
            validPassword,
            validToken
        );
        var result = this._validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("p")]
    [InlineData("password")]
    [InlineData("password1")]
    [InlineData("password!")]
    [InlineData("Password")]
    [InlineData("Password1")]
    [InlineData("Password!")]
    [InlineData("More_Than_Sixteen_Digits_Long_1!")]
    public void Should_Have_Error_When_Password_Is_Invalid(
        string invalidPassword
    )
    {
        var command = new ResetPasswordCommand(
            validEmail,
            invalidPassword,
            validToken
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
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
        var command = new ResetPasswordCommand(
            validEmail,
            validPassword,
            invalidToken
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_Token_And_OldPassword_Not_Provided()
    {
        var command = new ResetPasswordCommand(
            validEmail,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_OldPassword_And_NewPassword_Are_Equals()
    {
        var command = new ResetPasswordCommand(
            validEmail,
            validPassword,
            null,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Token);
    }

    [Fact]
    public void Should_Have_Error_When_OldPassword_And_Token_Provided()
    {
        var command = new ResetPasswordCommand(
            validEmail,
            "newPassword123!",
            validToken,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Token);
        result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
