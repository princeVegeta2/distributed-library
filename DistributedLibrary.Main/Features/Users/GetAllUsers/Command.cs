using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Users.GetAllUsers
{
    internal sealed record GetAllUsersCommand() : IRequest<Result<List<GetAllUsersResult>>>;
}
