using Restaurants.Domain.Authorization.Enums;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Authorization.Interfaces;

public interface IRestaurantAuthorizationService
{
    bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);
}
