using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public class StoreConfiguration : AuditableEntityConfiguration<Store>
{
    public override void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasOne(s => s.Seller)
            .WithOne(seller => seller.Store)
            .HasForeignKey<Store>(s => s.SellerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.Status != StoreStatusTypes.Rejected);
    }
}