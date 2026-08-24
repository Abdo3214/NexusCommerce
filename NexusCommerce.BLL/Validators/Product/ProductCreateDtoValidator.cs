using FluentValidation;
using NexusCommerce.BLL.DTOs.Product;

namespace NexusCommerce.BLL.Validators.Product
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
