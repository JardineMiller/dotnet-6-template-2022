using Mapster;

namespace Template.Api.Common.Mapping;

public class DefaultMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddConfig(config);
    }

    public static void AddConfig(TypeAdapterConfig config)
    {
        AddTrim(config);
    }

    private static void AddTrim(TypeAdapterConfig config)
    {
        config.Default.AddDestinationTransform(
            (string x) => x.Trim()
        );
    }
}
