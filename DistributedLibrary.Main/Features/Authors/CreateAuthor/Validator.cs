using FluentValidation;

namespace DistributedLibrary.Main.Features.Authors.CreateAuthor
{
    internal sealed class CreateAuthorValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorValidator()
        {
            RuleFor(x => x.Name).Must(name => !string.IsNullOrWhiteSpace(name)).MaximumLength(128).WithMessage("Name cannot be empty or longer than 128 characters");
        }
    }
}
