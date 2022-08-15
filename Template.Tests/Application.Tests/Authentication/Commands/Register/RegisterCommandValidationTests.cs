using Template.Application.Authentication.Commands.Register;
using Xunit;
using FluentValidation.TestHelper;

namespace Template.Application.Tests.Application.Tests.Authentication.Commands.Register;

public class RegisterCommandValidationTests
{
    private readonly RegisterCommandValidation _validator;
    private const string validPassword = "Password123!";
    private const string validFirstName = "Test";
    private const string validLastName = "User";
    private const string validEmail = "test@user.com";

    public RegisterCommandValidationTests()
    {
        this._validator = new RegisterCommandValidation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
    )]
    public void Should_Have_Error_When_FirstName_Is_Invalid(
        string invalidFirstName
    )
    {
        var command = new RegisterCommand(
            invalidFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
    )]
    public void Should_Have_Error_When_LastName_Is_Invalid(
        string invalidLastName
    )
    {
        var command = new RegisterCommand(
            validFirstName,
            invalidLastName,
            validEmail,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("just_a_random_string")]
    public void Should_Have_Error_When_Email_Is_Invalid(
        string invalidEmail
    )
    {
        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            invalidEmail,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
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
        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            invalidPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_With_Valid_Input()
    {
        var command = new RegisterCommand(
            validFirstName,
            validLastName,
            validEmail,
            validPassword
        );

        var result = this._validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
