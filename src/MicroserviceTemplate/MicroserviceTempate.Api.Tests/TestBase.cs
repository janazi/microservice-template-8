using MicroserviceTemplate.Api.IoC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;
using Testcontainers.PostgreSql;

namespace MicroserviceTemplate.Api;

[TestClass]
public class TestBase
{
    private static PostgreSqlContainer? _postgresSqlContainer;
    private static IConfiguration _configuration;
    private static IServiceCollection _services;

    [AssemblyInitialize]
    public static void RunBeforeAnyTests(TestContext context)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.IntegrationTest.json", false, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        //mock hosting environment
        var hostingEnvironment = Mock.Of<IWebHostEnvironment>(w => w.EnvironmentName == "Development");
        hostingEnvironment.ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _services = new ServiceCollection();
        _services.AddSingleton(hostingEnvironment);
        _services.AddSingleton(_configuration);
        _services.AddLogging();
        _services.ConfigureIoC();

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(_ => _.HttpContext.User.Identity.Name).Returns(Guid.Parse("a28b7ca1-54ba-4f55-b5b3-d8d18e17c3ba").ToString());

        _services.Replace(mockHttpContextAccessor.Object);

        InitializePostgres();
    }

    private static void InitializePostgres()
    {
        _postgresSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithPortBinding(5432, 5432)
            .WithPassword("my-password")
            .WithName("ms-template-postgres")
            .Build();
        _postgresSqlContainer.StartAsync().GetAwaiter().GetResult();
    }
}