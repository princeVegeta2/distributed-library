using DistributedLibrary.Main.Features.Authors.CreateAuthor;
using DistributedLibrary.Main.Features.Authors.DeleteAuthor;
using DistributedLibrary.Main.Features.Authors.GetAuthor;
using DistributedLibrary.Main.Features.Authors.ModifyAuthor;

namespace DistributedLibrary.Main.Features.Authors._Endpoints
{
    internal static class AuthorEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorEndpoints(this IEndpointRouteBuilder app)
            => app
                .MapCreateAuthorEndpoint()
                .MapGetAuthorEndpoint()
                .MapModifyAuthorEndpoint()
                .MapDeleteAuthorEndpoint();
    }
}
