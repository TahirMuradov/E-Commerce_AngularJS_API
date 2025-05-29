using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
    public class SizeProduct:BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
        public int StockQuantity { get; set; }
    }
}
