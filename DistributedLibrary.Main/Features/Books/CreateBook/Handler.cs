using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.CreateBook
{
    internal sealed class CreateBookHandler : IRequestHandler<CreateBookRequest, Result<Guid>>
    {
        private readonly AppDbContext _db;

        public CreateBookHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<Guid>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
        {
            var exists = await _db.Set<Book>().AnyAsync(b => b.Title.ToLower() == request.Title.ToLower(), cancellationToken);
            if (exists)
                return Result<Guid>.Conflict("A book by that title already exists");

            var book = new Book(Guid.NewGuid(), request.Title, request.PublishedAt, request.AuthorId);

            try
            {
                _db.Set<Book>().Add(book);
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Guid>.Exception("A concurrency exception was raised");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Exception($"Something went wrong: {ex.Message}");
            }

            return Result<Guid>.Success(book.Id);
        }
    }
}
