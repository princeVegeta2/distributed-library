using DistributedLibrary.Main.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLibrary.Main.Features.Users.GetAllUsers
{
    internal static class GetAllUsersEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllUsersEndpoint (this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/all", async (
                [FromServices] ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new GetAllUsersCommand(), ct);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return result.Status switch
                {
                    ResultStatus.NotFound => Results.NotFound(new { message = result.ErrorMessage }),
                    _ => Results.BadRequest(new { message = "Something went wrong" })
                };
            })
                .WithSummary("Get all users")
                .Produces<List<GetAllUsersResult>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
            return app;
        }
    }
}
