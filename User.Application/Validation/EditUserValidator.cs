using FluentValidation;
using User.Application.DTO;
using User.Domain.Modal;

namespace User.Application.Validation
{
    public class EditUserValidator : AbstractValidator<UserModal>
    {
        public EditUserValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required").GreaterThan(0);
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Role);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Address);
            RuleFor(x => x.PostalCode);
            RuleFor(x => x.Country);
            RuleFor(x => x.Phone).Length(10);
        }
    }
}
