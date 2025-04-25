using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int RegularPrice { get; set; }
        public int? SalePrice { get; set; }
    }
}
