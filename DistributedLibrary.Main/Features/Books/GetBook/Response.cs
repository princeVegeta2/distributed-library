namespace DistributedLibrary.Main.Features.Books.GetBook
{
    internal sealed record GetBookResponse(
        Guid Id,
        string Title,
        DateTimeOffset PublishedAt,
        GetBookAuthor Author);

    internal sealed record GetBookAuthor(
        Guid Id,
        string Name);
}
