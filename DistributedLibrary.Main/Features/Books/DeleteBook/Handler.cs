using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.DeleteBook
{
    internal sealed class DeleteBookHandler : IRequestHandler<DeleteBookQuery, Result>
    {
        private readonly AppDbContext _db;

        public DeleteBookHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(DeleteBookQuery request, CancellationToken cancellationToken)
        {
            int rowsAffected = await _db.Set<Book>().Where(b => b.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            if (rowsAffected == 0)
                return Result.NotFound("A book with this ID does not exist");

            return Result.Success();
        }
    }
}
