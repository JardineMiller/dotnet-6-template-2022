using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Template.Application.Common.Interfaces.Services;

namespace Template.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _user;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._user = httpContextAccessor.HttpContext?.User!;
    }

    public bool IsAuthenticated =>
        this._user.Identity?.IsAuthenticated ?? false;

    public string? UserName =>
        this.IsAuthenticated ? this._user.Identity?.Name : null;

    public string? UserId =>
        this.IsAuthenticated
            ? this._user.Claims
              .FirstOrDefault(
                  x => x.Type == ClaimTypes.NameIdentifier
              )
              ?.Value
            : null;
}
