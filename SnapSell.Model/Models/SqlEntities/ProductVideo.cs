using SnapSell.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class ProductVideo:Auditable
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid VideoId { get; set; }
        public virtual Video Video { get; set; }
    }
}
