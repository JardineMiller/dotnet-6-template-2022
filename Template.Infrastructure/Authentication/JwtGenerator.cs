using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Services;
using Template.Domain.Entities;

namespace Template.Infrastructure.Authentication;

public class JwtGenerator : IJwtGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(
        IDateTimeProvider dateTimeProvider,
        IOptions<JwtSettings> jwtSettings
    )
    {
        this._dateTimeProvider = dateTimeProvider;
        this._jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(this._jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256
        );
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        var securityToken = new JwtSecurityToken(
            issuer: this._jwtSettings.Issuer,
            audience: this._jwtSettings.Audience,
            expires: this._dateTimeProvider.UtcNow.AddMinutes(
                this._jwtSettings.ExpiryMinutes
            ),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(
            securityToken
        );
    }
}