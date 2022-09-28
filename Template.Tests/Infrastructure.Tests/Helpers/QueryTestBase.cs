using System;
using Template.Infrastructure.Persistence;

namespace Template.Application.Tests.Infrastructure.Tests.Helpers;

public class QueryTestBase : IDisposable
{
    public ApplicationDbContext Context { get; set; }

    public QueryTestBase()
    {
        this.Context = TestDbContextFactory.Create();
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(this.Context);
    }
}
