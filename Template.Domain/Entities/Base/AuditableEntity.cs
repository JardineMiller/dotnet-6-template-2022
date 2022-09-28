using Template.Domain.Interfaces;

namespace Template.Domain.Entities.Base;

public class AuditableEntity : BaseEntity, IAuditable
{
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}
