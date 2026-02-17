using DistributedLibrary.Main.Domain;
using Microsoft.EntityFrameworkCore;

namespace DistributedLibrary.Main.Infrastructure.DB
{
    /// <summary>
    /// This static class is used to seed the database in Program.cs
    /// </summary>
    internal static class DBSeeder
    {
        /// <summary>
        /// The async seeding task
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task SeedAsync(AppDbContext db, CancellationToken ct)
        {
            // First check whether we already have an author. If we do the database already has data, return
            if (await db.Set<Author>().AnyAsync(ct))
                return;

            var author1 = new Author(Guid.NewGuid(), "J.R.R Tolkien");
            var author2 = new Author(Guid.NewGuid(), "Frank Herbert");

            db.Set<Author>().AddRange(author1, author2);

            // Now create books and add them to their respective authors
            db.Set<Book>().AddRange(
                new Book(Guid.NewGuid(), "Fellowship of the Ring", new DateTimeOffset(1954, 7, 29, 0, 0, 0, TimeSpan.Zero), author1.Id),
                new Book(Guid.NewGuid(), "Two Towers", new DateTimeOffset(1954, 11, 11, 0, 0, 0, TimeSpan.Zero), author1.Id),
                new Book(Guid.NewGuid(), "Dune", new DateTimeOffset(1965, 8, 1, 0, 0, 0, TimeSpan.Zero), author2.Id),
                new Book(Guid.NewGuid(), "Dune: Book Two", new DateTimeOffset(1969, 2, 1, 0, 0, 0, TimeSpan.Zero), author2.Id)
            );

            // Now add two users
            var user1 = new User(Guid.NewGuid(), "CoolUser", "cool-email@gmail.com");
            var user2 = new User(Guid.NewGuid(), "UncoolUser", "not-cool-email@gmail.com");
            user1.SetPasswordHash("justanexamplepasswordhash");
            user2.SetPasswordHash("justanotherpasswordhashexample");

            db.Set<User>().AddRange(user1, user2);

            await db.SaveChangesAsync(ct);
        }
    }
}
