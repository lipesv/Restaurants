
using Restaurants.Domain.Services.Storage;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
                                                IRestaurantsRepository restaurantsRepository,
                                                IRestaurantAuthorizationService restaurantAuthorizationService,
                                                IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {

        logger.LogInformation("Uploading restaurant logo for Id: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbiddenException();

        var logoUrl = await blobStorageService.UploadFileToBlobAsync(request.File, request.FileName);
        restaurant.LogoUrl = logoUrl;

        await restaurantsRepository.SaveChanges();
    }
}
