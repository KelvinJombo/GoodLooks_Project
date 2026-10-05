using Catalog.Application.Dtos.Service;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
    {
        public UpdateServiceDtoValidator()
        {
            RuleFor(x => x.StyleName).NotEmpty().WithMessage("Product name must be provided").MaximumLength(50).WithMessage("Product name must not exceed 50 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product Description must be provided").MaximumLength(200).WithMessage("Description Text must not exceed 200 characters");
            RuleFor(x => x.PriceTag).NotEmpty().WithMessage("Product Price must be provided").GreaterThan(0).WithMessage("Product Price must be above zero");            
        }
    }
}
