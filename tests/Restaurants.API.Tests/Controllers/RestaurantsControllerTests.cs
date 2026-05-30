using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task GetAllRestaurants_ShouldReturn200_ForValidRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllRestaurants_ShouldReturn400_ForInvalidRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/restaurants");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.BadRequest);
    }
}