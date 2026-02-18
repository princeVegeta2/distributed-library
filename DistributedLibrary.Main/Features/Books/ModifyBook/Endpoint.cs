using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Books.ModifyBook
{
    internal static class ModifyBookEndpoint
    {
        public static IEndpointRouteBuilder MapModifyBookEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/books/{id:guid}", async (
                [FromRoute] Guid id,
                [FromBody] ModifyBookRequest req,
                [FromServices] IValidator<ModifyBookRequest> validator,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var val = await validator.ValidateAsync(req, ct);
                if (!val.IsValid)
                    return Results.ValidationProblem(val.ToDictionary());

                var result = await sender.Send(new ModifyBookCommand(id, req), ct);
                if (result.IsSuccess)
                    return Results.NoContent();

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Modifies a book by id")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesValidationProblem();
            return app;
        }
    }
}
