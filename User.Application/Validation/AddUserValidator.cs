using FluentValidation;
using User.Application.DTO;

namespace User.Application.Validation
{
    public class AddUserValidator : AbstractValidator<UserDTO>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Address);
            RuleFor(x => x.PostalCode);
            RuleFor(x => x.Country);
            RuleFor(x => x.Phone).Length(10);
        }
    }
}
