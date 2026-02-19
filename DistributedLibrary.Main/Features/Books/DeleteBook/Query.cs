using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.DeleteBook
{
    internal sealed record DeleteBookQuery(Guid Id) : IRequest<Result>;
}
