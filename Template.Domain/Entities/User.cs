using Microsoft.AspNetCore.Identity;
using Template.Domain.Interfaces;

namespace Template.Domain.Entities;

public class User : IdentityUser, IAuditable
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}
