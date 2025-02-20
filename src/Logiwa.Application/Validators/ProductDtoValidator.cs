using FluentValidation;
using Logiwa.Application.Models.Product;

namespace Logiwa.Application.Validators;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(product => product.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to 0.")
            .NotEmpty().WithMessage("StockQuantity is required.");

        RuleFor(product => product.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(product => product.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be a non-negative integer.");
    }
}
