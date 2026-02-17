using DistributedLibrary.Main.Common;
using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Infrastructure.DB;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Features.Users.SignUp
{
    internal sealed class SignupHandler : IRequestHandler<SignupRequest, Result<Guid>>
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public SignupHandler(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public async Task<Result<Guid>> Handle(SignupRequest request, CancellationToken cancellationToken)
        {
            var usernameExists = await _db.Set<User>().AnyAsync(x => x.Username == request.Username, cancellationToken);
            if (usernameExists)
                return Result<Guid>.Conflict("The username is already taken");
            var emailExists = await _db.Set<User>().AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result<Guid>.Conflict("The email is already taken");

            var user = new User(Guid.NewGuid(), request.Username, request.Email);

            var passwordHash = _hasher.HashPassword(user, request.Password);
            user.SetPasswordHash(passwordHash);

            try
            {
                _db.Set<User>().Add(user);
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException concurrency)
            {
                return Result<Guid>.Exception(concurrency.Message);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Exception(ex.Message);
            }

            return Result<Guid>.Success(user.Id);
        }
    }
}
