using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Authorization.Enums;
using Restaurants.Domain.Authorization.Interfaces;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantHandler(ILogger<DeleteDishesForRestaurantHandler> logger,
                                              IRestaurantsRepository restaurantsRepository,
                                              IDishesRepository dishesRepository,
                                              IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesForRestaurant>
{
    public async Task Handle(DeleteDishesForRestaurant request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing all dishes from restaurant with Id: {RestaurantId}", request.restaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.restaurantId);

        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.restaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException();

        await dishesRepository.DeleteAsync(restaurant.Dishes);
    }
}