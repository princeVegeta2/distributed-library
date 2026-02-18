using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Authors.ModifyAuthor
{
    internal sealed record ModifyAuthorRequest(string Name);
}
