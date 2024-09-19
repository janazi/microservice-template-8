using MicroserviceTemplate.Api;
using MicroserviceTemplate.Infra.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace RulesEngine.Api.IntegrationTests;

[CollectionDefinition(nameof(PostgressCollection))]
public class PostgressCollection : ICollectionFixture<PostgresFixture>;


[Collection(nameof(PostgressCollection))]
public class VehiclesControllerTests(CustomWebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    private readonly CustomWebApplicationFactory<Program> webApplicationFactory = webApplicationFactory;


    [Collection(nameof(PostgressCollection))]
    public class Get(CustomWebApplicationFactory<Program> webApplicationFactory) : VehiclesControllerTests(webApplicationFactory)
    {

        private const string Endpoint = "/api/1.0/vehicles";
        private readonly CustomWebApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;

        [Fact]
        public async Task ShouldRespondWith200ContainingACollectionOfRulesetDto()
        {
            var sp = _webApplicationFactory._services!.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<ApiDbContext>();

            //await dbContext.Rulesets.AddAsync(ruleset);
            //await dbContext.SaveChangesAsync();

            // Arrange
            // Act
            var result = await _httpClient.GetAsync($"{Endpoint}/{Guid.NewGuid()}");
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}