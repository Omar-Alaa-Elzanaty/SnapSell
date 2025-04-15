using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Entities;

namespace SnapSell.Presistance.EntityConfiguration
{
    public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ProductId)
                .HasMaxLength(50);

            builder.Property(r => r.Title)
                .HasMaxLength(100);

            builder.Property(r => r.Comment)
                .HasMaxLength(2000);

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);
        }
    }
}
