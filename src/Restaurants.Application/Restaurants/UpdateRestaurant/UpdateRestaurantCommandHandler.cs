using Restaurants.Application.Restaurants.CreateRestaurant;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper,
                                            IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with Id: {RestaurantId} with {@UpdatedRestaurant}",
                              command.Id,
                              command);

        var restaurantToUpdate = await restaurantsRepository.GetByIdAsync(command.Id);

        if (restaurantToUpdate is null)
            throw new NotFoundException(nameof(Restaurant), command.Id.ToString());


        if (!restaurantAuthorizationService.Authorize(restaurantToUpdate, ResourceOperation.Update))
            throw new ForbiddenException();

        mapper.Map(command, restaurantToUpdate);

        await restaurantsRepository.SaveChanges();

        logger.LogInformation("Restaurant with Id: {RestaurantId} updated successfully", command.Id);
    }
}
