using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Seller
    {
        public string Id { get; set; }
        public virtual Account Account { get; set; }
    }
}
