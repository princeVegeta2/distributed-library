using FluentValidation;

namespace DistributedLibrary.Main.Features.Books.CreateBook
{
    internal sealed class CreateBookValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.Title).Must(title => !string.IsNullOrWhiteSpace(title)).MaximumLength(128).WithMessage("Title cannot be empty our longer than 128 chars");
            RuleFor(x => x.PublishedAt).NotEmpty().LessThan(DateTimeOffset.UtcNow).WithMessage("PublishedAt cannot be empty or in the future");
        }
    }
}
