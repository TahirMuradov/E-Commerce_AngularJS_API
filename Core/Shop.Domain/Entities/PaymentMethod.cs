using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class PaymentMethod:BaseEntity
    {
        public bool IsCash { get; set; }
        public List<PaymentMethodLanguages> PaymentMethodLanguages { get; set; }

    }
}
