using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Entities;

namespace SnapSell.Presistance.EntityConfiguration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.StockQuantity)
                .HasDefaultValue(0);

            builder.Property(i => i.ReservedQuantity)
                .HasDefaultValue(0);

            //indexe
            builder.HasIndex(i => i.ProductId)
                .IsUnique();
        }
    }
}
