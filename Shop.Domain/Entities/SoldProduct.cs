using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
    public class SoldProduct:BaseEntity
    {

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    
        public decimal SoldPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime SoldTime { get; set; }
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
