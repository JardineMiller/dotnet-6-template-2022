using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Template.Infrastructure.Services;

namespace Template.Infrastructure.Persistence;

public class DbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public const string ConnectionString =
        "Server=localhost;Database=Template;Trusted_Connection=True;MultipleActiveResultSets=true";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new ApplicationDbContext(
            optionsBuilder.Options,
            new DateTimeProvider()
        );
    }
}
