using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ProductConfiguration : AuditableEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EnglishName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ArabicName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(x => x.Categories)
            .WithMany()
            .UsingEntity(j => j.ToTable("ProductCategories"));

        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(x => x.IsHidden)
            .HasDefaultValue(false);

        builder.Property(p => p.MainImageUrl)
            .IsRequired(false);

        builder.Property(p => p.MainVideoUrl)
            .IsRequired(false);

        builder.HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductPaymentMethods)
            .WithOne(ppm => ppm.Product)
            .HasForeignKey(ppm => ppm.ProductId);

        builder.Property(x => x.ProductStatus)
            .HasConversion<string>()
            .IsRequired(false);

        builder.Property(x => x.ShippingType)
            .HasConversion<string>()
            .IsRequired(false);
    }
}