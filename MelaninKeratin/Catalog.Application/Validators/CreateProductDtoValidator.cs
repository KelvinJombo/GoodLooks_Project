using Catalog.Application.Dtos.Product;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name must be provided").MaximumLength(50).WithMessage("Product name must not exceed 50 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product Description must be provided").MaximumLength(200).WithMessage("Description Text must not exceed 200 characters");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Product Price must be provided").GreaterThan(0).WithMessage("Product Price must be above zero");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand name must be provided").MaximumLength(50).WithMessage("Brand name letters must not exceed 50");
        }
    }
}
