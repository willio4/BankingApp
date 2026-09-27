using BankingApp.Infrastructure.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).IsRequired();
            builder.Property(t => t.UserId).IsRequired();
            builder.Property(t => t.Token).IsRequired().HasMaxLength(500);
            builder.Property(t => t.ExpiresAt).IsRequired();
            builder.Property(t => t.IsRevoked).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();

            builder.HasIndex(t => t.Token).IsUnique();

            builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}