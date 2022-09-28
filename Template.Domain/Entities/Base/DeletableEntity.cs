using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities.Base;

public class DeletableEntity : AuditableEntity, IDeletable
{
    public DateTimeOffset? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
