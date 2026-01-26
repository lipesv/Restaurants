using MediatR;

namespace Restaurants.Application.Restaurants.CreateRestaurant;

public record CreateRestaurantCommand(bool HasDelivery,
                                      string? ContactEmail,
                                      string? ContactNumber,
                                      string? City,
                                      string? Street,
                                      string? PostalCode) : IRequest<int>
{
    public CreateRestaurantCommand() : this(false, null, null, null, null, null) { }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;    
}
