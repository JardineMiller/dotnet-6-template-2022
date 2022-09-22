using ErrorOr;

namespace Template.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail = Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use"
        );

        public static Error CreationFailed = Error.Failure(
            code: "User.CreationFailed",
            description: "Failed to create user"
        );
    }
}
