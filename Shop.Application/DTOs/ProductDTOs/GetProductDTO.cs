namespace Shop.Application.DTOs.ProductDTOs
{
   public class GetProductDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Title { get; set; }

        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; }


    }
}
