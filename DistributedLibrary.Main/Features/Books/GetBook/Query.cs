using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.GetBook
{
    internal sealed record GetBookQuery(Guid Id) : IRequest<Result<GetBookResponse>>;
}
