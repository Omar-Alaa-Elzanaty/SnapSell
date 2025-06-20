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
        
        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<Seller>("Seller")
            .HasValue<Client>("Client");
    }
}