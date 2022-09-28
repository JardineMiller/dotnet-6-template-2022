using System;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Template.Infrastructure.Persistence;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Persistence;

public class DbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_WithConnectionString_ReturnsDbContext()
    {
        // Arrange
        var factory = new DbContextFactory();

        // Act
        using var context = factory.CreateDbContext(
            Array.Empty<string>()
        );

        // Assert
        context.Database.EnsureCreated();
        context.Database
            .GetConnectionString()
            .ShouldBe(DbContextFactory.ConnectionString);
    }
}
