using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class Client
    {
        public string Id { get; set; }
        public virtual Account Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string About { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual List<OrderAddress> Addresses { get; set; } = [];

        public virtual List<Review> Reviews { get; set; } = [];
    }
}