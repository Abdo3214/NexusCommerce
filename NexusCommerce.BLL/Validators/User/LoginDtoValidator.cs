using FluentValidation;
using NexusCommerce.BLL.DTOs.User;

namespace NexusCommerce.BLL.Validators.User
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
