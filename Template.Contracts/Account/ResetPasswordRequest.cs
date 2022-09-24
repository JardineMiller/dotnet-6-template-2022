namespace Template.Contracts.Account;

public record ResetPasswordRequest(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
);
