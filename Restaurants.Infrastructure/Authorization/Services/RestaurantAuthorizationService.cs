using Microsoft.Extensions.Logging;
using Restaurants.Application.Context.Interface;
using Restaurants.Domain.Authorization.Enums;
using Restaurants.Domain.Authorization.Interfaces;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
                                            IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Authorizing user {UserEmail} for {ResourceOperation} on restaurant {RestaurantId}",
                              user!.Email,
                              resourceOperation,
                              restaurant.Name);

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read operations - successful authorization.");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole("Admin"))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization.");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner, delete/update operation - successful authorization.");
            return true;
        }

        logger.LogInformation("Authorization failed.");
        return false;
    }
}
