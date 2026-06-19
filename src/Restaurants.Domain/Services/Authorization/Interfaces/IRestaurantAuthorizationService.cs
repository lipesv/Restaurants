using Restaurants.Domain.Authorization.Enums;

namespace Restaurants.Domain.Authorization.Interfaces;

public interface IRestaurantAuthorizationService
{
    bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);
}
