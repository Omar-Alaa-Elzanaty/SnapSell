using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class PaymentMethodConfiguration : AuditableEntityConfiguration<PaymentMethod>
{
    public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        base.Configure(builder);

        builder.HasMany(p => p.ProductPaymentMethods)
            .WithOne(ppm => ppm.PaymentMethod)
            .HasForeignKey(ppm => ppm.PaymentMethodId);
    }
}