using System;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Services;

namespace Template.Application.Tests.Infrastructure.Tests.Helpers;

public class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        var user1 = new User()
        {
            Id = "0001",
            UserName = "User 1",
            Email = "user1@test.com",
            EmailConfirmed = true
        };

        var user2 = new User()
        {
            Id = "0002",
            UserName = "User 2",
            Email = "user2@test.com",
            EmailConfirmed = true
        };

        var context = new ApplicationDbContext(
            options,
            new DateTimeProvider()
        );

        context.Database.EnsureCreated();
        context.Users.AddRange(user1, user2);
        context.SaveChanges();
        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();

        context.Dispose();
    }
}
