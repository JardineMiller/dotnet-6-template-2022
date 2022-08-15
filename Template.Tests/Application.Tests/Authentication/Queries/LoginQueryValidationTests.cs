using FluentValidation.TestHelper;
using Template.Application.Authentication.Queries.Login;
using Xunit;

namespace Template.Application.Tests.Application.Tests.Authentication.Queries;

public class LoginQueryValidationTests
{
    private readonly LoginQueryValidation _validator;

    private const string validPassword = "Password123!";
    private const string validEmail = "test@user.com";

    public LoginQueryValidationTests()
    {
        this._validator = new LoginQueryValidation();
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
        var query = new LoginQuery(invalidEmail, validPassword);
        var result = this._validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("a")]
    [InlineData("passs")]
    private void Should_Have_Error_When_Password_Is_Invalid(
        string invalidPassword
    )
    {
        var query = new LoginQuery(validEmail, invalidPassword);
        var result = this._validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    private void Should_Pass_When_Valid_Input_Provided()
    {
        var query = new LoginQuery(validEmail, validPassword);
        var result = this._validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
