using DistributedLibrary.Main.Features.Users.SignUp;

namespace DistributedLibrary.Main.Features.Users._Endpoints
{
    internal static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
            => app
                .MapSignupEndpoint();
    }
}
