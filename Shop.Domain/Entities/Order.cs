using Shop.Domain.Entities.Common;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities
{
  public  class Order:BaseEntity
    {

        public string OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string OrderPdfPath { get; set; }
        public decimal ShippingPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<SoldProduct>  SoldProducts { get; set; }
        public Guid ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
     
    }
}
