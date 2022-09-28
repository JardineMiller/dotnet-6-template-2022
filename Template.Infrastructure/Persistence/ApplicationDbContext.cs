using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Domain.Entities.Interfaces;

namespace Template.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new()
    )
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = this.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTimeOffset.Now;
                    break;
                case EntityState.Deleted:
                    if (entry.Entity is IDeletable deletableEntity)
                    {
                        deletableEntity.DeletedOn =
                            DateTimeOffset.Now;
                        deletableEntity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }
    }
}
