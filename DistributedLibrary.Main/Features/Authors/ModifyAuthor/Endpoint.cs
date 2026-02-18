using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Authors.ModifyAuthor
{
    internal static class ModifyAuthorEndpoint
    {
        public static IEndpointRouteBuilder MapModifyAuthorEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/authors/{id:guid}", async (
                [FromRoute] Guid id,
                [FromBody] ModifyAuthorRequest req,
                [FromServices] IValidator<ModifyAuthorRequest> validator,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var val = await validator.ValidateAsync(req, ct);
                if (!val.IsValid)
                    return Results.ValidationProblem(val.ToDictionary());

                var result = await sender.Send(new ModifyAuthorCommand(id, req), ct);
                if (result.IsSuccess)
                    return Results.NoContent();

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Modifies an author")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesValidationProblem();

            return app;
        }
    }
}
