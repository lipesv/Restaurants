namespace Restaurants.Application.Restaurants.GetRestaurants;

public class GetRestaurantsQueryValidator : AbstractValidator<GetRestaurantsQuery>
{
    private readonly int[] allowedPageSizes = [5, 10, 15, 30];
    private readonly string[] allowedSortByValues =
    [
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Category),
        nameof(RestaurantDto.Description)
    ];


    public GetRestaurantsQueryValidator()
    {

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1)
                                  .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize).Must(value => allowedPageSizes.Contains(value))
                                .WithMessage($"Page size must be in: {string.Join(", ", allowedPageSizes)}.");

        RuleFor(x => x.SortBy).Must(value => allowedSortByValues.Contains(value))
                              .When(x => !string.IsNullOrEmpty(x.SortBy))
                              .WithMessage($"Sort is optional, or must be in: {string.Join(", ", allowedSortByValues)}.");
    }

}