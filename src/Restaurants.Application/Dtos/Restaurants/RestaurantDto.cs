namespace Restaurants.Application.Dtos.Restaurants;

public record RestaurantDto(int Id, bool HasDelivery, string? City, string? Street, string? PostalCode)
{
    public RestaurantDto() : this(default, default, default, default, default) { }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string? LogoSasUrl { get; set; }

    public List<DishDto> Dishes { get; set; } = [];
}