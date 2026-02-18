using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.CreateBook
{
    internal sealed record CreateBookRequest(Guid AuthorId, string Title, DateTimeOffset PublishedAt) : IRequest<Result<Guid>>;
}
