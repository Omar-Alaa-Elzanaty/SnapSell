using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration
{
    internal class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasOne(x => x.Account)
                .WithOne()
                .HasForeignKey<Seller>(x => x.Id);
        }
    }
}
