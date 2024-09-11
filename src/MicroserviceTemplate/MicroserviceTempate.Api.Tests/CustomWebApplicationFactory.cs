using MicroserviceTemplate.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RulesEngine.Api.IntegrationTests;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public IServiceCollection? _services;
    public IServiceScopeFactory _scopeFactoryForTest;
    public IServiceCollection _servicesForTest;
    private object lockObject = new object();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            EnsureDatabase(services);
            services.BuildServiceProvider();
            _services = services;
            _scopeFactoryForTest = _services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        });
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.IntegrationTest.json"), optional: false, reloadOnChange: true);
        });
        builder.UseEnvironment("Development");
    }

    protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        if (_scopeFactoryForTest == null)
            throw new InvalidOperationException("Scope factory for test is null");

        using var scope = _scopeFactoryForTest.CreateScope();
        var result = await action(scope.ServiceProvider).ConfigureAwait(false);
        return result;
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<ApiDbContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<ApiDbContext>()));

    public async Task InsertAsync<T>(T entity) where T : class
    {
        await ExecuteDbContextAsync(async db =>
        {
            db.Set<T>().Add(entity);

            return await db.SaveChangesAsync();
        });
    }

    private static void EnsureDatabase(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApiDbContext>();

        dbContext!.Database.Migrate();
    }
}