namespace Shop.Application.DTOs.ShippingMethodDTOs
{
  public  class GetShippingMethodDTO
    {
        public Guid Id { get; set; }
        public string content { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
    }
}
