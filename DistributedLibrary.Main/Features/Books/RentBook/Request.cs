using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.RentBook
{
    internal sealed record RentBookRequest(Guid BookId, Guid UserId, DateTimeOffset RentUntil) : IRequest<Result>;
}
