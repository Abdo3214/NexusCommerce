using FluentValidation;
using NexusCommerce.BLL.DTOs.Category;

namespace NexusCommerce.BLL.Validators.Category
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
