using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces.Services;
using Template.Domain.Entities;
using Template.Domain.Entities.Interfaces;

namespace Template.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeProvider dateTimeProvider
    ) : base(options)
    {
        this._dateTimeProvider = dateTimeProvider;
    }

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
                    entry.Entity.CreatedOn =
                        this._dateTimeProvider.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedOn =
                        this._dateTimeProvider.Now;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is IDeletable deletableEntity)
                    {
                        deletableEntity.DeletedOn =
                            this._dateTimeProvider.Now;
                        deletableEntity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }
    }
}
