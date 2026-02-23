using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Books.RentBook
{
    internal static class RentBookEndpoint
    {
        public static IEndpointRouteBuilder MapRentBookEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/books/rent", async (
                [FromBody] RentBookRequest req,
                [FromServices] IValidator<RentBookRequest> validator,
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var val = await validator.ValidateAsync(req, ct);
                if (!val.IsValid)
                    return Results.ValidationProblem(val.ToDictionary());

                var result = await sender.Send(req, ct);
                if (result.IsSuccess)
                    return Results.Ok();

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Rents a book")
                .Produces(StatusCodes.Status200OK)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
            return app;
        }
    }
}
