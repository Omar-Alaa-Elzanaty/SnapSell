using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public class StoreConfiguration : AuditableEntityConfiguration<Store>
{
    public override void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasOne(s => s.Account)
            .WithOne(seller => seller.Store)
            .HasForeignKey<Store>(s => s.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => x.Status != StoreStatusTypes.Rejected);
    }
}