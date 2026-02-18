using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Authors.CreateAuthor
{
    internal static class CreateAuthorEndpoint
    {
        public static IEndpointRouteBuilder MapCreateAuthorEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/authors/", async (
                [FromBody] CreateAuthorRequest req,
                [FromServices] IValidator<CreateAuthorRequest> validator,
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
                        $"/api/authors/{id}",
                        new { id });
                }

                return result.Status switch
                {
                    ResultStatus.Conflict => Results.Conflict(new { message = result.ErrorMessage }),
                    ResultStatus.Exception => Results.UnprocessableEntity(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Creates an author")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
                .ProducesProblem(StatusCodes.Status400BadRequest);
            return app;
        }
    }
}
