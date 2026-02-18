using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Authors.GetAuthor
{
    internal sealed class GetAuthorHandler : IRequestHandler<GetAuthorQuery, Result<GetAuthorResponse>>
    {
        private readonly AppDbContext _db;

        public GetAuthorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<GetAuthorResponse>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _db.Set<Author>().AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new GetAuthorResponse(
                    a.Id,
                    a.Name,
                    a.Books
                    .Select(b => new GetAuthorBook(
                        b.Id,
                        b.Title,
                        b.PublishedAt))
                    .ToList()))
                .SingleOrDefaultAsync(cancellationToken);

            if (author is null)
                return Result<GetAuthorResponse>.NotFound("An author with this ID does not exist");

            return Result<GetAuthorResponse>.Success(author);
        }
    }
}
