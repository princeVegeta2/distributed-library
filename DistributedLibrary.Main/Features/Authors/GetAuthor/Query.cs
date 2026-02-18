using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Authors.GetAuthor
{
    internal sealed record GetAuthorQuery(Guid Id) : IRequest<Result<GetAuthorResponse>>;
}
