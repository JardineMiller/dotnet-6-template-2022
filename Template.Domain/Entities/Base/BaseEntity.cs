using Template.Domain.Interfaces;

namespace Template.Domain.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}
