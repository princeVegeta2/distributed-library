using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Authors.CreateAuthor
{
    internal sealed class CreateAuthorHandler : IRequestHandler<CreateAuthorRequest, Result<Guid>>
    {
        private readonly AppDbContext _db;

        public CreateAuthorHandler (AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<Guid>> Handle(CreateAuthorRequest request, CancellationToken cancellationToken)
        {
            var exists = await _db.Set<Author>().AnyAsync(a => a.Name.ToLower() == request.Name.ToLower(), cancellationToken);
            if (exists)
                return Result<Guid>.Conflict("An author by that name already exists");

            var author = new Author(Guid.NewGuid(), request.Name);

            try
            {
                _db.Set<Author>().Add(author);
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Guid>.Exception("Concurrency violated. Please try again");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Exception($"Something went wrong: {ex.Message}");
            }

            return Result<Guid>.Success(author.Id);
        }
    }
}
