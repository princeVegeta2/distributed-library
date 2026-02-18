using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Books.CreateBook
{
    internal static class CreateBookEndpoint
    {
        public static IEndpointRouteBuilder MapCreateBookEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/books/", async (
                [FromBody] CreateBookRequest req,
                [FromServices] IValidator<CreateBookRequest> validator,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var val = await validator.ValidateAsync(req, ct);
                if (!val.IsValid)
                    return Results.ValidationProblem(val.ToDictionary());

                var result = await sender.Send(req, ct);

                if (result.IsSuccess)
                {
                    var id = result.Value;
                    return Results.Created(
                        $"/api/books/{id}",
                        new { id });
                }

                return result.Status switch
                {
                    ResultStatus.Conflict => Results.Conflict(new { message = result.ErrorMessage }),
                    ResultStatus.Exception => Results.UnprocessableEntity(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Creates a book")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
                .ProducesProblem(StatusCodes.Status400BadRequest);
            return app;
        }
    }
}
