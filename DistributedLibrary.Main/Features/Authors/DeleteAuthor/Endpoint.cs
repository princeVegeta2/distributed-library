using DistributedLibrary.Main.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Authors.DeleteAuthor
{
    internal static class DeleteAuthorEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteAuthorEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/authors/{id:guid}", async (
                [FromRoute] Guid id,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteAuthorQuery(id), ct);
                if (result.IsSuccess)
                    return Results.NoContent();

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Deletes an author by it's id")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);
            return app;
        }
    }
}
