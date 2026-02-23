using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain.Users;
using DistributedLibrary.Main.Features.Books.GetAllBooks;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Users.GetAllUsers
{
    internal sealed class GetAllBooksHandler : IRequestHandler<GetAllUsersCommand, Result<List<GetAllUsersResult>>>
    {
        private readonly AppDbContext _db;

        public GetAllBooksHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<List<GetAllUsersResult>>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
        {
            var users = await _db.Set<User>().AsNoTracking()
                .Select(u => new GetAllUsersResult(
                    u.Id,
                    u.Username))
                .ToListAsync(cancellationToken);
            if (users.Count == 0)
                return Result<List<GetAllUsersResult>>.NotFound("No users currently exist");

            return Result<List<GetAllUsersResult>>.Success(users);
        }
    }
}
