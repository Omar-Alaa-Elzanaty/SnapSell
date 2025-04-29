using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public class OrderConfiguration : AuditableEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShippingAddress)
            .WithMany()
            .HasForeignKey(x => x.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BillingAddress)
            .WithMany()
            .HasForeignKey(x => x.BillingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.OrderTotal)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}