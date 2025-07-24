using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class ShippingMethod:BaseEntity
    {

        public decimal Price { get; set; }
        public decimal DisCountPrice { get; set; }
        public List<ShippingMethodLanguage> ShippingMethodLanguages { get; set; }
    }
}
