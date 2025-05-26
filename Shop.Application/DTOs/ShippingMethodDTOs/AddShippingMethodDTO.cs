namespace Shop.Application.DTOs.ShippingMethodDTOs
{
   public class AddShippingMethodDTO
    {
        public Dictionary<string,string> Content { get; set; }
        public decimal Price { get; set; }
        public decimal DisCountPrice { get; set; }
    }
}
