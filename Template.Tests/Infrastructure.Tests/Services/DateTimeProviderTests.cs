using System;
using Shouldly;
using Template.Infrastructure.Services;
using Xunit;

namespace Template.Application.Tests.Infrastructure.Tests.Services;

public class DateTimeProviderTests
{
    private readonly DateTimeProvider _dateTimeProvider = new();

    [Fact]
    public void Provider_Returns_Current_DateTime()
    {
        var now = this._dateTimeProvider.UtcNow;

        now.ShouldNotBe(DateTime.MinValue);
        now.ShouldNotBe(DateTime.MaxValue);

        now.Date.ShouldBeEquivalentTo(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Provider_Returns_Current_DateTimeOffset()
    {
        var now = this._dateTimeProvider.Now;

        now.ShouldNotBe(DateTimeOffset.MinValue);
        now.ShouldNotBe(DateTimeOffset.MaxValue);

        now.Date.ShouldBeEquivalentTo(DateTimeOffset.Now.Date);
    }
}
