using DistributedLibrary.Main.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedLibrary.Main.Infrastructure.DB.Configurations
{
    internal sealed class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> b)
        {
            b.HasKey(x => x.Id);

            // Properties
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.CreatedAt).IsRequired().ValueGeneratedNever();
            b.Property(x => x.UpdatedAt).IsRequired(false).ValueGeneratedNever();

            // Readonly navigation
            b.Navigation(x => x.Books).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
