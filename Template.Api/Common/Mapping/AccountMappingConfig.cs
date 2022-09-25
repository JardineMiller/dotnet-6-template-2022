using Mapster;
using Template.Application.Account.Commands.RequestResetPassword;
using Template.Application.Account.Commands.ResetPassword;
using Template.Application.Account.Common;
using Template.Contracts.Account.RequestResetPassword;
using Template.Contracts.Account.ResetPassword;

namespace Template.Api.Common.Mapping;

public class AccountMappingConfig
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<ResetPasswordRequest, ResetPasswordCommand>()
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.Token),
                dest => dest.Token
            )
            .IgnoreIf(
                (src, dest) => string.IsNullOrEmpty(src.OldPassword),
                dest => dest.OldPassword
            );

        config.NewConfig<
            ResetPasswordResult,
            ResetPasswordResponse
        >();

        config.NewConfig<
            RequestResetPasswordRequest,
            RequestResetPasswordCommand
        >();

        config.NewConfig<
            RequestResetPasswordResult,
            RequestResetPasswordResponse
        >();
    }
}
