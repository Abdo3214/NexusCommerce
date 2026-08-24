using FluentValidation;
using NexusCommerce.BLL.DTOs.Image;

namespace NexusCommerce.BLL.Validators.Image
{
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {
        public ImageUploadDtoValidator()
        {
            RuleFor(x => x.File).NotNull();
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("File cannot be empty.");
            RuleFor(x => x.File.ContentType).Must(x => x != null && x.StartsWith("image/")).WithMessage("Only image files are allowed.");
        }
    }
}
