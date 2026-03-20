using Restaurants.Application.Dishes.CreateDish;

namespace Restaurants.Application.Dtos.Profiles;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>().ReverseMap();
    }
}