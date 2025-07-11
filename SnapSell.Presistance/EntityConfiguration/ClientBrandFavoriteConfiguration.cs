using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ClientBrandFavoriteConfiguration : IEntityTypeConfiguration<ClientBrandFavorite>
{
    public void Configure(EntityTypeBuilder<ClientBrandFavorite> builder)
    {
        builder.ToTable("ClientBrandFavorites");
        
        builder.HasKey(x => new { x.ClientId, x.BrandId });
        
        builder.HasOne(x => x.Client)
            .WithMany(x => x.FavoriteBrands)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Property(x => x.AddedDate)
            .IsRequired();
    }
}