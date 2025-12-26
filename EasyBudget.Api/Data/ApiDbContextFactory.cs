using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyBudget.Api.Data;

/// <summary>
/// Design-time factory for EF Core migrations.
/// This bypasses the full app startup (avoiding Swashbuckle and other incompatibilities).
/// </summary>
public class ApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.Load(".env.local");

        var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
        ArgumentException.ThrowIfNullOrEmpty(dbName, "DATABASE_NAME environment variable is not set");
        var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
        ArgumentException.ThrowIfNullOrEmpty(dbUser, "DATABASE_NAME environment variable is not set");
        var dbPass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        ArgumentException.ThrowIfNullOrEmpty(dbPass, "DATABASE_NAME environment variable is not set");

        var connectionString = $"Host=localhost;Database={dbName};Username={dbUser};Password={dbPass}";

        var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        // Pass null for encryption service - migrations don't need encryption converters
        return new ApiDbContext(optionsBuilder.Options, null);
    }
}
