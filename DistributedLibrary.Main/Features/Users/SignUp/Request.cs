using DistributedLibrary.Main.Common;
using MediatR;

namespace DistributedLibrary.Main.Features.Users.SignUp
{
    internal sealed record SignupRequest(string Username, string Email, string Password) : IRequest<Result<Guid>>;
}
