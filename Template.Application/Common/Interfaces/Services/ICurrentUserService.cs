namespace Template.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserName { get; }
    string? UserId { get; }
}
