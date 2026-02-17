using DistributedLibrary.Main.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedLibrary.Main.Infrastructure.DB.Configurations
{
    internal sealed class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(x => x.Id);

            // Properties
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Username).HasMaxLength(128).IsRequired();
            b.Property(x => x.Email).HasMaxLength(128).IsRequired().IsConcurrencyToken();
            b.Property(x => x.PasswordHash).HasMaxLength(2000).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired().ValueGeneratedNever();
            b.Property(x => x.UpdatedAt).IsRequired(false).ValueGeneratedNever();
            b.Property(x => x.Version).IsConcurrencyToken(); // We are setting the ID in application level(constructor of User)
        }
    }
}
