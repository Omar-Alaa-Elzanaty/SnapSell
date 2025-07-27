using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Presistance.EntityConfiguration;

public class AccountConfiguration:IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.LastUpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(x => x.LastUpdatedBy)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
        
        builder.Property(x => x.Country)
            .IsRequired(false);
        
        builder.Property(x => x.About)
            .IsRequired(false);

        builder.Property(x => x.Gender)
            .IsRequired(false);

        builder.Property(x => x.BirthDate)
            .IsRequired(false);

    }
}