using Template.Domain.Entities;

namespace Template.Application.Authentication.Common;

public record AuthenticationResult(User User, string? Token = null);
