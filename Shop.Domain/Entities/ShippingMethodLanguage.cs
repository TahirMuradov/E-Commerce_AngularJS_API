namespace Shop.Domain.Entities
{
   public class ShippingMethodLanguage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string LangCode { get; set; }
        public Guid ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
    }
}
