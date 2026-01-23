namespace Restaurants.Application.Dtos.Restaurants;

public record CreateRestaurantDto(bool HasDelivery, string? ContactEmail, string? ContactNumber, string? City, string? Street, string? PostalCode)
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
}
