using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ClientBrandFavoriteConfiguration : IEntityTypeConfiguration<ClientBrandFavorite>
{
    public void Configure(EntityTypeBuilder<ClientBrandFavorite> builder)
    {
        builder.ToTable("ClientBrandFavorites");
        
        builder.HasKey(x => new { x.AccountId, x.BrandId });
        
        builder.HasOne(x => x.Account)
            .WithMany(x => x.FavoriteBrands)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}