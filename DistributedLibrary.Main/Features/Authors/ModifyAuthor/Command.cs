using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Authors.ModifyAuthor
{
    internal sealed record ModifyAuthorCommand(Guid Id, ModifyAuthorRequest req) : IRequest<Result>;
}
