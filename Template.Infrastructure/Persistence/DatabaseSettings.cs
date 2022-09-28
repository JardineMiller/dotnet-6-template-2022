namespace Template.Infrastructure.Persistence;

public class DatabaseSettings
{
    public static readonly string SectionName = "Database";
    public string? ConnectionString { get; set; }
}
