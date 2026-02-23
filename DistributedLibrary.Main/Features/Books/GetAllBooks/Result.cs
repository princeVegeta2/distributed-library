namespace DistributedLibrary.Main.Features.Books.GetAllBooks
{
    internal sealed record GetAllBooksResult(
        Guid Id,
        string Title,
        DateTimeOffset PublishedAt,
        Guid AuthorId);
}
