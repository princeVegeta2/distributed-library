using DistributedLibrary.Main.Features.Books.CreateBook;
using DistributedLibrary.Main.Features.Books.GetBook;
using DistributedLibrary.Main.Features.Books.ModifyBook;

namespace DistributedLibrary.Main.Features.Books._Endpoints
{
    internal static class BookEndpoints
    {
        public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
            => app
                .MapCreateBookEndpoint()
                .MapGetBookEndpoint()
                .MapModifyBookEndpoint();
    }
}
