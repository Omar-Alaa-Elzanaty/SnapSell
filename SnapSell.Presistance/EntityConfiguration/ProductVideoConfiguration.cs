using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapSell.Domain.Models.SqlEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Presistance.EntityConfiguration
{
    public class ProductVideoConfiguration : IEntityTypeConfiguration<ProductVideo>
    {
        public void Configure(EntityTypeBuilder<ProductVideo> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.VideoId });
        }
    }
}
