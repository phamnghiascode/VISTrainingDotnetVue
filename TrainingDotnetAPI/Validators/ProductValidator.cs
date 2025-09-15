using FluentValidation;
using TrainingDotnetAPI.DTOs;

namespace TrainingDotnetAPI.Validators
{
    public class ProductValidator : AbstractValidator<UpsertProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");    
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(9999999).WithMessage("Price must be less than or equal to 9,999,999.");

        }
    }
}
