namespace Restaurants.Application.Dishes.GetDishById;

public record GetDishByIdQuery(int RestaurantId, int DishId) : IRequest<DishDto>;
