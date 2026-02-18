using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Authors.ModifyAuthor
{
    internal sealed class ModifyAuthorHandler : IRequestHandler<ModifyAuthorCommand, Result>
    {
        private readonly AppDbContext _db;

        public ModifyAuthorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(ModifyAuthorCommand c, CancellationToken cancellationToken)
        {
            var author = await _db.Set<Author>().SingleOrDefaultAsync(a => a.Id == c.Id, cancellationToken);
            if (author is null)
                return Result.NotFound("An author with this ID does not exist");

            author.ChangeName(c.req.Name);
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
