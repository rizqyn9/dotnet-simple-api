using FluentValidation;
using SampleApi.DTOs;

namespace SampleApi.Validators
{
  public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
  {
    public CreateUserDtoValidator()
    {
      RuleFor(x => x.Username)
          .NotEmpty().WithMessage("Username is required.")
          .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

      RuleFor(x => x.Email)
          .NotEmpty().EmailAddress().WithMessage("Valid email is required.");

      RuleFor(x => x.Password)
          .NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");

      RuleFor(x => x.Role)
          .Must(r => r == "admin" || r == "user")
          .WithMessage("Role must be either 'admin' or 'user'.");
    }
  }
}
