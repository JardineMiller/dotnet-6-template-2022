using Microsoft.AspNetCore.Identity;

namespace Template.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}
