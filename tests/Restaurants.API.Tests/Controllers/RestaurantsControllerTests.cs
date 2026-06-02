using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.API.Tests.Handlers;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Repositories;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new();


    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                // services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                services.AddAuthorization();

                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantRepositoryMock.Object));
            }));
    }

    [Fact]
    public async Task GetAllRestaurants_ShouldReturn200_ForValidRequest()
    {
        // Arrange
        var client = CreateAnonymousClient();

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
        var client = CreateAnonymousClient();

        // Act
        var response = await client.GetAsync("/api/restaurants");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRestaurantById_WhenUserIsAuthenticated_ShouldReturn404_ForNonExistentRestaurant()
    {
        // Arrange
        const int id = 9999;

        _restaurantRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                 .ReturnsAsync((Restaurant?)null);

        var client = CreateAuthenticatedClient("German");

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetRestaurantById_WhenUserIsAuthenticated_ShouldReturn200_ForExistingRestaurant()
    {
        // Arrange
        const int id = 1;

        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test Restaurant",
            Description = "A test restaurant"
        };

        _restaurantRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                 .ReturnsAsync(restaurant);

        var client = CreateAuthenticatedClient("German");

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.OK);

        restaurantDto.Should()
                     .NotBeNull();

        restaurantDto.Name.Should()
                          .Be(restaurant.Name);

        restaurantDto.Description.Should()
                                 .Be(restaurant.Description);
    }

    [Fact]
    public async Task GetRestaurantById_WhenUserIsNotAuthenticated_ShouldReturn401()
    {
        // Arrange
        const int id = 1;

        var client = CreateAnonymousClient();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetRestaurantById_WhenUserHasNoPermissions_ShouldReturn403()
    {
        // Arrange
        const int id = 1;

        var client = CreateAuthenticatedClient();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should()
                           .Be(System.Net.HttpStatusCode.Forbidden);
    }

    private HttpClient CreateAuthenticatedClient(string? nationality = null)
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        if (!string.IsNullOrWhiteSpace(nationality))
        {
            client.DefaultRequestHeaders.Add("X-Test-Nationality", nationality);
        }

        return client;
    }

    private HttpClient CreateAnonymousClient()
    {
        return _factory.CreateClient();
    }
}