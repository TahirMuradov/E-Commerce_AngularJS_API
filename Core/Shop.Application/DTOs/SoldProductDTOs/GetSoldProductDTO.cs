namespace Shop.Application.DTOs.SoldProductDTOs
{
   public class GetSoldProductDTO
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal SoldPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime SoldTime { get; set; }
        public string Size { get; set; }
        public string OrderNumber { get; set; }
        public Guid OrderId { get; set; }
        public string OrderPath { get; set; }
        public Guid ProductId { get; set; }


    }
}
