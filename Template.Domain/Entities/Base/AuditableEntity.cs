using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities.Base;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}
