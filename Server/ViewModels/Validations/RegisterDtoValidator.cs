using Server.Controllers;
using FluentValidation;
using FluentValidation.Attributes;

namespace Server.Controllers
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(dto => dto.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(dto => dto.Firstname).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(dto => dto.Name).NotEmpty().WithMessage("LastName cannot be empty");
        }
    }

    [Validator(typeof(RegisterDtoValidator))]
    public partial class RegisterDto
    {
    }
}
