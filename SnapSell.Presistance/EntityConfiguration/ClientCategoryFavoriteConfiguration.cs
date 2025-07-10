using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ClientCategoryFavoriteConfiguration : IEntityTypeConfiguration<ClientCategoryFavorite>
{
    public void Configure(EntityTypeBuilder<ClientCategoryFavorite> builder)
    {
        builder.ToTable("ClientCategoryFavorites");
        
        builder.HasKey(x => new { x.ClientId, x.CategoryId });
        
        builder.HasOne(x => x.Client)
            .WithMany(x => x.FavoriteCategories)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Property(x => x.AddedDate)
            .IsRequired();
    }
}