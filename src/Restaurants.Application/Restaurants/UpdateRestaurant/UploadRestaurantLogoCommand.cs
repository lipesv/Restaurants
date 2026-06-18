namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public record UploadRestaurantLogoCommand(int RestaurantId, string FileName, Stream File) : IRequest;