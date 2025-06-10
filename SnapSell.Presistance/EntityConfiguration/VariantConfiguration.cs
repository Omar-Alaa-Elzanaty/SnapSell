using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Presistance.EntityConfiguration;
public sealed class VariantConfiguration : AuditableEntityConfiguration<Variant>
{
    public override void Configure(EntityTypeBuilder<Variant> builder)
    {
        base.Configure(builder);

        builder.ToTable("Variants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        //builder.HasOne(x => x.Product)
        //    .WithMany(x => x.Variants)
        //    .HasForeignKey(x => x.ProductId)
        //    .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.SalePrice)
            .HasColumnType("decimal(18,2)");
    }
}