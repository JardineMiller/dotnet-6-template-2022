using Mapster;

namespace Template.Api.Common.Mapping;

public class DefaultMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.AddDestinationTransform(
            (string x) => x.Trim()
        );
    }
}