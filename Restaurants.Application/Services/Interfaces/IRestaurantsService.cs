using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Services.Interfaces
{
    public interface IRestaurantsService
    {
        Task<IEnumerable<RestaurantDto>> GetAllRestaurants();

        Task<RestaurantDto?> GetRestaurantById(int id);

        Task<int> Create(CreateRestaurantDto createRestaurantDto);
    }
}