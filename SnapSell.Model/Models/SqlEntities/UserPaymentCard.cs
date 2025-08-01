using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.Interfaces;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Domain.Models.SqlEntities
{
    public class UserPaymentCard : Auditable
    {
        public string UserId { get; set; }
        public virtual Account User { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public string Token { get; set; }
        public string CardSubType { get; set; }
        public string MaskedPan { get; set; }
    }
}
