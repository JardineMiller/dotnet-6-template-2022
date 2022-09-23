using ErrorOr;

namespace Template.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials = Error.Validation(
            code: "Auth.InvalidCredentials",
            description: "Invalid credentials."
        );

        public static Error EmailNotConfirmed = Error.Validation(
            code: "Auth.EmailNotConfirmed",
            description: "Email not confirmed."
        );
    }
}
