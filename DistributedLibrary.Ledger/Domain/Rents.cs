namespace DistributedLibrary.Ledger.Domain
{
    internal sealed class Rents
    {
        public Guid Id { get; private set; } = default!;
        public Guid UserId { get; private set; } = default!;
        public Guid BookId { get; private set; } = default!;
        public DateTimeOffset RentedAt { get; private set; } = default!;
        public DateTimeOffset ExpiresAt { get; private set; } = default!;
        public DateTimeOffset? ReturnedAt { get; private set; } = null;
        public bool IsReturned { get; private set; } = false;

        private Rents() { }

        public Rents(Guid id, Guid userId, Guid bookId, DateTimeOffset rentedAt, DateTimeOffset expiresAt)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
            RentedAt = rentedAt;
            ExpiresAt = expiresAt;
        }

        public void Return()
        {
            IsReturned = true;
            ReturnedAt = DateTimeOffset.UtcNow;
        }
    }
}
