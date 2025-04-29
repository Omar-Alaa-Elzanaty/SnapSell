using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Auditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.LastUpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(x => x.LastUpdatedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}