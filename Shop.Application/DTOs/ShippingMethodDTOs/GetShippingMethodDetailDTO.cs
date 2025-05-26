namespace Shop.Application.DTOs.ShippingMethodDTOs
{
   public class GetShippingMethodDetailDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string,string> content { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
    }
}
