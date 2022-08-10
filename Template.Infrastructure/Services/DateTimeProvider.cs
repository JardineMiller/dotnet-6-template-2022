using Template.Application.Common.Interfaces.Services;

namespace Template.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.Now;
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}