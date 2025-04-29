using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ReviewConfiguration : AuditableEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);

        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Score)
            .HasColumnType("decimal(2,1)");

        builder.Property(x => x.Content)
            .HasMaxLength(2000);
    }
}