using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.GetAllBooks
{
    internal sealed record GetAllBooksCommand() : IRequest<Result<List<GetAllBooksResult>>>;
}
