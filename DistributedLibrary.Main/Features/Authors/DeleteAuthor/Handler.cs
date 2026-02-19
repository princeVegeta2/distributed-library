using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Authors.DeleteAuthor
{
    internal sealed class DeleteAuthorHandler : IRequestHandler<DeleteAuthorQuery, Result>
    {
        private readonly AppDbContext _db;

        public DeleteAuthorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(DeleteAuthorQuery request, CancellationToken cancellationToken)
        {
            int rowsAffected = await _db.Set<Author>().Where(a => a.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            if (rowsAffected == 0)
                return Result.NotFound("An author with this ID does not exist");

            return Result.Success();
        }
    }
}
