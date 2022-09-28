using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}
