using FluentValidation;

namespace Restaurants.Application.Restaurants.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian", "Portuguese"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Restaurant name is required.")
            .Length(3, 100)
            .WithMessage("Restaurant name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.ContactEmail))
            .WithMessage("Please provide a valid email address.");

        RuleFor(x => x.ContactNumber)
            .Matches(@"^\(?\d{3}\)?[\s\-]?\d{3}\-?\d{4}$")
            .When(x => !string.IsNullOrEmpty(x.ContactNumber))
            .WithMessage("Please provide a valid contact number (XXX) XXX-XXXX.");

        RuleFor(x => x.PostalCode)
            .Matches(@"^[0-9]{5}(?:-[0-9]{4})?$")
            .When(x => !string.IsNullOrEmpty(x.PostalCode))
            .WithMessage("Please provide a valid postal code XXXXX-XXXX.");
    }
}
