using AutoMapper;
using Restaurants.Application.Dishes.CreateDish;
using Restaurants.Application.Dtos.Dishes;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos.Profiles;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>().ReverseMap();
    }
}