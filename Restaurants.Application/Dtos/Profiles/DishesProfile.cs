using AutoMapper;
using Restaurants.Application.Dtos.Dishes;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos.Profiles;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>().ReverseMap();
    }
}