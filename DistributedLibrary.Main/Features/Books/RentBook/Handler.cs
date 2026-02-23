using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain.Books;
using DistributedLibrary.Main.Domain.Books.Events;
using DistributedLibrary.Main.Domain.Users;
using DistributedLibrary.Main.Infrastructure.DB;
using DistributedLibrary.Main.Infrastructure.Webhooks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Books.RentBook
{
    internal sealed class RentBookHandler : IRequestHandler<RentBookRequest, Result>
    {
        private readonly AppDbContext _db;
        private readonly IWebhookDispatcher _dispatcher;

        public RentBookHandler(AppDbContext db, IWebhookDispatcher dispatcher)
        {
            _db = db;
            _dispatcher = dispatcher;
        }

        public async Task<Result> Handle(RentBookRequest request, CancellationToken cancellationToken)
        {
            var bookExists = await _db.Set<Book>().AsNoTracking().AnyAsync(b => b.Id == request.BookId);
            if (!bookExists)
                return Result.NotFound("A book with this ID does not exist");
            var userExists = await _db.Set<User>().AsNoTracking().AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
                return Result.NotFound("A user with this ID does not exist");

            var evt = new BookRentedEvent(request.UserId, request.BookId, DateTimeOffset.UtcNow, request.RentUntil);
            await _dispatcher.EnqueueWebhookAsync(BookRentedEvent.EventType, evt);

            return Result.Success();
        }
    }
}
