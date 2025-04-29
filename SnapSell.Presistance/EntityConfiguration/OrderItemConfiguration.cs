using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class OrderItemConfiguration : AuditableEntityConfiguration<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Variant)
            .WithMany()
            .HasForeignKey(x => x.VariantId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ProductVariantUnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Ignore(x => x.TotalPrice);

    }
}