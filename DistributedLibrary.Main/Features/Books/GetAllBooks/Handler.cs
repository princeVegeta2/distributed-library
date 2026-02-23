using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain.Books;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.GetAllBooks
{
    internal sealed class GetAllBooksHandler : IRequestHandler<GetAllBooksCommand, Result<List<GetAllBooksResult>>>
    {
        private readonly AppDbContext _db;

        public GetAllBooksHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<List<GetAllBooksResult>>> Handle(GetAllBooksCommand request, CancellationToken cancellationToken)
        {
            var books = await _db.Set<Book>().AsNoTracking()
                .Select(b => new GetAllBooksResult(
                    b.Id,
                    b.Title,
                    b.PublishedAt,
                    b.AuthorId))
                .ToListAsync(cancellationToken);
            if (books.Count == 0)
                return Result<List<GetAllBooksResult>>.NotFound("No books exist");

            return Result<List<GetAllBooksResult>>.Success(books);
        }
    }
}
