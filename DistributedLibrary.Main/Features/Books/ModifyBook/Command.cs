using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Books.ModifyBook
{
    internal sealed record ModifyBookCommand(Guid Id, ModifyBookRequest Request) : IRequest<Result>;
}
