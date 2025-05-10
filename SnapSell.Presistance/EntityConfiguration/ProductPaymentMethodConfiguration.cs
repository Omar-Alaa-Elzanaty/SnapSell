using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ProductPaymentMethodConfiguration : IEntityTypeConfiguration<ProductPaymentMethod>
{
    public void Configure(EntityTypeBuilder<ProductPaymentMethod> builder)
    {
        builder.ToTable("ProductPaymentMethods");
        builder.HasKey(ppm => new { ppm.ProductId, ppm.PaymentMethodId });

        builder.HasOne(ppm => ppm.Product)
            .WithMany(p => p.ProductPaymentMethods)
            .HasForeignKey(ppm => ppm.ProductId);

        builder.HasOne(ppm => ppm.PaymentMethod)
            .WithMany(pm => pm.ProductPaymentMethods)
            .HasForeignKey(ppm => ppm.PaymentMethodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}