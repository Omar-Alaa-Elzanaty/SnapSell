using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ClientCategoryFavoriteConfiguration : IEntityTypeConfiguration<ClientCategoryFavorite>
{
    public void Configure(EntityTypeBuilder<ClientCategoryFavorite> builder)
    {
        builder.ToTable("ClientCategoryFavorites");
        
        builder.HasKey(x => new { x.AccountId, x.CategoryId });
        
        builder.HasOne(x => x.Account)
            .WithMany(x => x.FavoriteCategories)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}