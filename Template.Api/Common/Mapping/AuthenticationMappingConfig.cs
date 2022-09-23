using Mapster;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Authentication.Common;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;
using Template.Domain.Entities;

namespace Template.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config
            .NewConfig<AuthenticationResult, AuthenticationResponse>()
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.Token),
                dest => dest.Token
            )
            .Map(dest => dest, src => src.User);

        config
            .NewConfig<RegisterCommand, User>()
            .Map(dest => dest.UserName, src => src.Email);
    }
}
