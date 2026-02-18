using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.ModifyBook
{
    internal sealed class ModifyBookHandler : IRequestHandler<ModifyBookCommand, Result>
    {
        private readonly AppDbContext _db;

        public ModifyBookHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(ModifyBookCommand c, CancellationToken cancellationToken)
        {
            var book = await _db.Set<Book>().SingleOrDefaultAsync(b => b.Id == c.Id, cancellationToken);
            if (book is null)
                return Result.NotFound("A book with that Id does not exist");

            book.ModifyBook(c.Request.Title, c.Request.PublishedAt);
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
