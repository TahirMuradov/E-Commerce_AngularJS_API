namespace Shop.Application.DTOs.OrderPdfGeneratorDTOs
{
   public class OrderShippingMethodDTO
    {
        public Guid Id { get; set; }
        public string ShippingContent { get; set; }
        public decimal Price { get; set; }
    }
}
