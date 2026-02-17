using DistributedLibrary.Main.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Users.SignUp
{
    internal static class SignupEndpoint
    {
        public static IEndpointRouteBuilder MapSignupEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/sign-up/", async (
                [FromBody] SignupRequest req,
                [FromServices] IValidator<SignupRequest> validator,
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
                        $"/api/users/{id}",
                        new { id });
                }

                return result.Status switch
                {
                    ResultStatus.Conflict => Results.Conflict(new { message = result.ErrorMessage }),
                    ResultStatus.Exception => Results.UnprocessableEntity(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("User signup")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
            return app;
        }
    }
}
