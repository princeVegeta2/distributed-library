using DistributedLibrary.Main.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Books.GetBook
{
    internal static class GetBookEndpoint
    {
        public static IEndpointRouteBuilder MapGetBookEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/books/{id:guid}", async (
                [FromRoute] Guid id,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new GetBookQuery(id), ct);
                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Gets a book with it's author")
                .Produces<GetBookResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);
            return app;
        }
    }
}
