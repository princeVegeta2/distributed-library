using FluentValidation;

namespace DistributedLibrary.Main.Features.Authors.ModifyAuthor
{
    internal sealed class ModifyAuthorValidator : AbstractValidator<ModifyAuthorRequest>
    {
        public ModifyAuthorValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(128).WithMessage("Name cannot be empty or longer than 128 characters");
        }
    }
}
