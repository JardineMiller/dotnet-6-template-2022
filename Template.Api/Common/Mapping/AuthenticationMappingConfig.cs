using Mapster;
using Template.Application.Authentication.Commands.Register;
using Template.Application.Authentication.Common;
using Template.Application.Authentication.Queries.Login;
using Template.Contracts.Authentication;

namespace Template.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config
            .NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest, src => src.User);
    }
}