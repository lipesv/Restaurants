using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Services.Interfaces;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Services;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Fetching all restaurants");

        try
        {
            var restaurants = await restaurantsRepository.GetAllAsync();

            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation("Fetching restaurant with ID: {Id}", id);

        try
        {
            var restaurant = await restaurantsRepository.GetByIdAsync(id);

            var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);

            return restaurantDto;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<int> Create(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation("Creating a new restaurant");

        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);

        int id = await restaurantsRepository.CreateAsync(restaurant);

        return id;
    }
}
