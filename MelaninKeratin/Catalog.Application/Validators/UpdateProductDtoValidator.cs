using Catalog.Application.Dtos.Product;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {           
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product Description must be provided").MaximumLength(200).WithMessage("Description Text must not exceed 200 characters");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Product Price must be provided").GreaterThan(0).WithMessage("Product Price must be above zero");            
        }
    }
}
