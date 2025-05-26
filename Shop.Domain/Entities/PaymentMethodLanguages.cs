using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class PaymentMethodLanguages:BaseEntity
    {
        public string Content { get; set; }
        public string  LangCode { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
