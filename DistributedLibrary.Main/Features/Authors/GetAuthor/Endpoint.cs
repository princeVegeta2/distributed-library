using DistributedLibrary.Main.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Authors.GetAuthor
{
    internal static class GetAuthorEndpoint
    {
        public static IEndpointRouteBuilder MapGetAuthorEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/authors/{id:guid}", async (
                [FromRoute] Guid id,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new GetAuthorQuery(id), ct);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Get an author and their books")
                .Produces<GetAuthorResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);
            return app;
        }
    }
}
