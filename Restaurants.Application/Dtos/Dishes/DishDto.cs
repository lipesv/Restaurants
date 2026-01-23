namespace Restaurants.Application.Dtos.Dishes;

public record DishDto(int Id, decimal Price, int? KiloCalories)
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
