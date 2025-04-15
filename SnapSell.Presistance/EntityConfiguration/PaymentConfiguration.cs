using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Entities;

namespace SnapSell.Presistance.EntityConfiguration
{
    public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(p => p.Status)
                .HasMaxLength(20);

            builder.Property(p => p.TransactionId)
                .HasMaxLength(100);

            builder.Property(p => p.FailureReason)
                .HasMaxLength(500);

            builder.HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId);

            //index
            builder.HasIndex(p => p.OrderId);
            builder.HasIndex(p => p.TransactionId).IsUnique();
        }
    }
}
