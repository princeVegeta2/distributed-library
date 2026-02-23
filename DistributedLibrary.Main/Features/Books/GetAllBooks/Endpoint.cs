using DistributedLibrary.Main.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Books.GetAllBooks
{
    internal static class GetAllBooksEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllBooksEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/books/all", async (
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new GetAllBooksCommand(), ct);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Get all books")
                .Produces<List<GetAllBooksResult>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
            return app;
        }
    }
}
