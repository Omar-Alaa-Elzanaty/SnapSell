using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ProductConfiguration : AuditableEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EnglishName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ArabicName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.EnglishDescription)
            .HasMaxLength(1000);

        builder.Property(x => x.ArabicDescription)
            .HasMaxLength(1000);

        builder.Property(x => x.Sku)
            .HasMaxLength(100);

        builder.HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Variants)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.HasIndex(x => x.StoreId);
        // builder.HasIndex(x => x.BrandId);
    }
}