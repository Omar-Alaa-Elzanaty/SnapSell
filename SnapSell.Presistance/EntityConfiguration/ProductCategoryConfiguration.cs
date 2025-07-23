using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration;

public sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(x => new { x.ProductId, x.CategoryId });
        
        builder.HasOne(x => x.Product)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.ProductId)
            .HasColumnType("int")
            .HasConversion<int>(); 

        builder.Property(x => x.CategoryId)
            .HasColumnType("uniqueidentifier");
        
        builder.Property(x => x.ParentCategoryId)
            .HasColumnName("ParentCategoryId")
            .HasColumnType("uniqueidentifier");
        
    }
}