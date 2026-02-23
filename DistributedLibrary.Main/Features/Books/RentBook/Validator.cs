using FluentValidation;

namespace DistributedLibrary.Main.Features.Books.RentBook
{
    internal sealed class RentBookValidator : AbstractValidator<RentBookRequest>
    {
        public RentBookValidator()
        {
            RuleFor(x => x.RentUntil).NotEmpty().GreaterThan(DateTimeOffset.UtcNow).WithMessage("RentUntil cannot be empty or in the past");
            RuleFor(x => x.RentUntil).LessThan(DateTimeOffset.UtcNow.AddMonths(1)).WithMessage("RentUntil cannot be more than 1 month in the future");
        }
    }
}
