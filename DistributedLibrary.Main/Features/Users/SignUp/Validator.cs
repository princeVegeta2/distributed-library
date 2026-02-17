using FluentValidation;

namespace DistributedLibrary.Main.Features.Users.SignUp
{
    internal sealed class SignupRequestValidator : AbstractValidator<SignupRequest>
    {
        public SignupRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty()
                .Must(username => !string.IsNullOrWhiteSpace(username))
                .MaximumLength(128)
                .WithMessage("Username cannot be empty or longer than 128 characters");

            RuleFor(x => x.Email).NotEmpty()
                .Must(email => !string.IsNullOrWhiteSpace(email))
                .MaximumLength(128)
                .EmailAddress()
                .WithMessage("Email cannot be empty, or longer than 128 characters. Must be a valid email");

            RuleFor(x => x.Password).NotEmpty()
                .Must(password => !string.IsNullOrWhiteSpace(password))
                .MinimumLength(6)
                .MaximumLength(200)
                .WithMessage("Password cannot be empty or longer than 200 characters. Minimum length: 6");
        }
    }
}
