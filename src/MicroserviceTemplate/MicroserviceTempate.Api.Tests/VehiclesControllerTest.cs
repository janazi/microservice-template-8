using MicroserviceTemplate.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RulesEngine.Api.IntegrationTests;
using System.Net;
using System.Text.Json;

namespace MicroserviceTemplate.Api;

[CollectionDefinition(nameof(PostgressCollection))]
public class PostgressCollection : ICollectionFixture<PostgresFixture>;

[Collection(nameof(PostgressCollection))]
public class VehiclesControllerTest(CustomWebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    public class Get(CustomWebApplicationFactory<Program> webApplicationFactory) : VehiclesControllerTest(webApplicationFactory)
    {

        private const string Endpoint = "/WeatherForecast";

        [Fact]
        public async Task Should_respond_a_status_200_ok()
        {
            // Arrange
            // Act
            var result = await _httpClient.GetAsync(Endpoint);
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Should_respond_a_list_of_weather_forecast()
        {
            // Act
            var result = await _httpClient.GetAsync(Endpoint);
            var serialized =
                JsonSerializer.Deserialize<List<Vehicle>>(await result.Content.ReadAsStringAsync());
            // Assert
            Assert.IsType<List<Vehicle>>(serialized);
        }
    }

    public class Post(CustomWebApplicationFactory<Program> webApplicationFactory)
        : VehiclesControllerTest(webApplicationFactory)
    {
        //TODO: IMPLEMENT POST TESTS
    }
}