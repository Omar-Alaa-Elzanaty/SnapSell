using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Presistance.EntityConfiguration;

internal class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        
        builder.HasOne(s => s.Store)
            .WithOne(store => store.Seller)
            .HasForeignKey<Store>(store => store.SellerId)
            .IsRequired(false); // Avoid circular issues during seeding if needed
    }
}