namespace DistributedLibrary.Main.Domain
{
    /// <summary>
    /// The Book Entity
    /// </summary>
    internal sealed class Book
    {
        public Guid Id { get; private set; } = default!;
        public string Title { get; private set; } = default!;
        public DateTimeOffset PublishedAt { get; private set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; private set; } = null;
        public Author Author { get; private set; } = default!;
        public Guid AuthorId { get; private set; } = default!;

        // EF
        private Book() { }

        public Book(Guid id, string title, DateTimeOffset publishedAt, Guid authorId)
        {
            Id = id;
            Title = title;
            PublishedAt = publishedAt;
            AuthorId = authorId;
        }

        public void ChangeTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title) && title != Title)
            {
                Title = title;
                UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
