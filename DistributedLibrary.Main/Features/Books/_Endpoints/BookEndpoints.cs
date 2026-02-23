using DistributedLibrary.Main.Features.Books.CreateBook;
using DistributedLibrary.Main.Features.Books.DeleteBook;
using DistributedLibrary.Main.Features.Books.GetAllBooks;
using DistributedLibrary.Main.Features.Books.GetBook;
using DistributedLibrary.Main.Features.Books.ModifyBook;
using DistributedLibrary.Main.Features.Books.RentBook;

namespace DistributedLibrary.Main.Features.Books._Endpoints
{
    internal static class BookEndpoints
    {
        public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
            => app
                .MapCreateBookEndpoint()
                .MapGetBookEndpoint()
                .MapModifyBookEndpoint()
                .MapDeleteBookEndpoint()
                .MapRentBookEndpoint()
                .MapGetAllBooksEndpoint();
    }
}
