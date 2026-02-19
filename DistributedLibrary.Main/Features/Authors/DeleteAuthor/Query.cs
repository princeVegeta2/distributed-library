using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Authors.DeleteAuthor
{
    internal sealed record DeleteAuthorQuery(Guid Id) : IRequest<Result>;
}
