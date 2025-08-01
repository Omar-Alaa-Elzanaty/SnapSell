using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Presistance.EntityConfiguration
{
    public class UserPaymentCardConfiguration : IEntityTypeConfiguration<UserPaymentCard>
    {
        public void Configure(EntityTypeBuilder<UserPaymentCard> builder)
        {
            builder.HasKey(x => new { x.UserId, x.Token });
        }
    }
}
