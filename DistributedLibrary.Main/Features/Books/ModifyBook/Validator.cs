using FluentValidation;

namespace DistributedLibrary.Main.Features.Books.ModifyBook
{
    internal sealed class ModifyBookValidator : AbstractValidator<ModifyBookRequest>
    {
        public ModifyBookValidator()
        {
            RuleFor(x => x).Must(HaveAtLeastOneValue)
                .WithMessage("The Request body must have at least one value to modify.");
            RuleFor(x => x.Title).NotEmpty().MaximumLength(128).When(x => x.Title is not null)
                .WithMessage("A title cannot be empty or longer than 128 characters.");
            RuleFor(x => x.PublishedAt).NotEmpty().LessThan(DateTimeOffset.UtcNow).When(x => x.PublishedAt is not null)
                .WithMessage("PublishedAt cannot be empty or in the future.");
        }

        private bool HaveAtLeastOneValue(ModifyBookRequest req) =>
            !string.IsNullOrWhiteSpace(req.Title) || req.PublishedAt is not null;
    }
}
