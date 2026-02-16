namespace DistributedLibrary.Main.Domain
{
    /// <summary>
    /// Author entity
    /// </summary>
    internal sealed class Author
    {
        public Guid Id { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; private set; } = null;
        private readonly List<Book> _books = [];
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        // EF
        private Author() { }

        public Author(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void ChangeName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name != Name)
            {
                Name = name;
                UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
