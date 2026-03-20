namespace Restaurants.Application.Dishes.GetDishesByRestaurantId;

public class GetDishesByRestaurantIdQueryHandler(ILogger<GetDishesByRestaurantIdQueryHandler> logger,
                                                 IRestaurantsRepository restaurantsRepository,
                                                 IMapper mapper) : IRequestHandler<GetDishesByRestaurantIdQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesByRestaurantIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dishes for restaurant with ID {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var results = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);

        return results;
    }
}
