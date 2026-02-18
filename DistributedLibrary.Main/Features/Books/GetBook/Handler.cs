using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.GetBook
{
    internal sealed class GetBookHandler : IRequestHandler<GetBookQuery, Result<GetBookResponse>>
    {
        private readonly AppDbContext _db;

        public GetBookHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<GetBookResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _db.Set<Book>().AsNoTracking()
                .Where(b => b.Id == request.Id)
                .Select(b => new GetBookResponse(
                    b.Id,
                    b.Title,
                    b.PublishedAt,
                    new GetBookAuthor(
                        b.Author.Id,
                        b.Author.Name)))
                .SingleOrDefaultAsync(cancellationToken);

            if (book is null)
                return Result<GetBookResponse>.NotFound("A book with that ID does not exist");

            return Result<GetBookResponse>.Success(book);
        }
    }
}
