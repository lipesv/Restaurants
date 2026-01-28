using FluentValidation;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Restaurant Id must be greater than zero.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Restaurant name is required.")
            .Length(3, 100)
            .WithMessage("Restaurant name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");        
    }
}
