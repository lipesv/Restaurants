using FluentValidation;

namespace Restaurants.Application.Dishes.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(50)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");

        RuleFor(x => x.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a non-negative number.");
    }
}
