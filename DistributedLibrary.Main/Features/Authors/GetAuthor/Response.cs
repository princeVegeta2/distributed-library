namespace DistributedLibrary.Main.Features.Authors.GetAuthor
{
    internal sealed record GetAuthorResponse(
        Guid Id,
        string Name,
        IReadOnlyCollection<GetAuthorBook> Books);

    internal sealed record GetAuthorBook(
        Guid Id,
        string Title,
        DateTimeOffset PublishedAt);
}
