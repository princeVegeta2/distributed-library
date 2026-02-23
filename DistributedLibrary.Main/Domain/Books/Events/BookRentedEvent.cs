using MediatR;

namespace DistributedLibrary.Main.Domain.Books.Events
{
    public sealed record BookRentedEvent(
        Guid UserId,
        Guid BookId,
        DateTimeOffset RentedAt,
        DateTimeOffset RentedUntil) :INotification
    {
        public const string EventType = "book_rented";
    }
}
