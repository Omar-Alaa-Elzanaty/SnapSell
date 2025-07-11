using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ReviewConfiguration : AuditableEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);

        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score)
            .HasColumnType("decimal(2,1)");

        builder.Property(x => x.Content)
            .HasMaxLength(2000);
    }
}