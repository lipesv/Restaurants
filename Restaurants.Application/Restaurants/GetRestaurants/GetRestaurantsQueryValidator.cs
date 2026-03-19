using FluentValidation;

namespace Restaurants.Application.Restaurants.GetRestaurants;

public class GetRestaurantsQueryValidator : AbstractValidator<GetRestaurantsQuery>
{
    private readonly int[] allowedPageSizes = new[] { 5, 10, 15, 30 };


    public GetRestaurantsQueryValidator()
    {

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1)
                                      .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize).Must(value => allowedPageSizes.Contains(value))
                                  .WithMessage($"Page size must be in: {string.Join(", ", allowedPageSizes)}.");
    }

}