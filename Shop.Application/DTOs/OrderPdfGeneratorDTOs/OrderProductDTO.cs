namespace Shop.Application.DTOs.OrderPdfGeneratorDTOs
{
    public class OrderProductDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
      
        public string size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
