using Testcontainers.PostgreSql;

namespace RulesEngine.Api.IntegrationTests;
public class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer? _postgresSqlContainer;

    public PostgresFixture()
    {
        _postgresSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithPortBinding(5432, 5432)
            .WithPassword("cm123")
            .Build();
    }


    public Task DisposeAsync() => _postgresSqlContainer!.StopAsync();

    public Task InitializeAsync() => _postgresSqlContainer!.StartAsync();
}
