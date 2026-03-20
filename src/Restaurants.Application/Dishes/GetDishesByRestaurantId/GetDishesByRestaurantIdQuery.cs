using MediatR;
using Restaurants.Application.Dtos.Dishes;

namespace Restaurants.Application.Dishes.GetDishesByRestaurantId;

public record GetDishesByRestaurantIdQuery(int RestaurantId) : IRequest<IEnumerable<DishDto>>;