namespace Template.Domain.Entities.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
}
