namespace Template.Application.Common.Patterns;

public static class Patterns
{
    public static class Auth
    {
        public const string Password =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";
    }
}
