using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Authors.CreateAuthor
{
    internal sealed record CreateAuthorRequest(string Name) : IRequest<Result<Guid>>;
}
