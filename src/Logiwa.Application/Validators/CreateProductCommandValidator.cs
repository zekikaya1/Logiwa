using FluentValidation;
using Logiwa.Application.Commands;
using Logiwa.Application.Models.Product;

namespace Logiwa.Application.Validators;

public class CreateProductCommandValidator  : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    

        RuleFor(product => product.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(product => product.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be a non-negative integer.");
    }
}
