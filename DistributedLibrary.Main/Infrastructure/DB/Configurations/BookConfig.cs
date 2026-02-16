using DistributedLibrary.Main.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedLibrary.Main.Infrastructure.DB.Configurations
{
    internal sealed class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> b)
        {
            b.HasKey(x => x.Id);

            // Properties
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.PublishedAt).IsRequired().ValueGeneratedNever();
            b.Property(x => x.CreatedAt).IsRequired().ValueGeneratedNever();
            b.Property(x => x.UpdatedAt).IsRequired(false).ValueGeneratedNever();

            // Reference to the book's author
            // Many:1 Books : Author
            b.HasOne(x => x.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
