using Api.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Api.FunctionalTests.TestInfrastructure;

public sealed class TestDatabase : IAsyncDisposable
{
    private readonly string databasePath = Path.Combine(Path.GetTempPath(), $"api-functional-tests-{Guid.NewGuid():N}.db");

    public string ConnectionString => new SqliteConnectionStringBuilder
    {
        DataSource = databasePath
    }.ToString();

    public async Task ResetAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public ValueTask DisposeAsync()
    {
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }

        return ValueTask.CompletedTask;
    }
}