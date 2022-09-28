using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities.Base;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}
