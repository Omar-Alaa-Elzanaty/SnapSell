using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class SizeConfiguration : AuditableEntityConfiguration<Size>
{
    public override void Configure(EntityTypeBuilder<Size> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.ParentSize)
            .WithMany()
            .HasForeignKey(x => x.ParentSizeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}